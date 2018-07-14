using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookClient.Data
{
  public class BookManager
  {
    const string Url = "http://xam150.azurewebsites.net/api/books/"; // URL del Servicio RESTful
    private string _authorizationKey; // Esta aplicación utiliza un método de autenticación por llave.

    /*
     * Inicializa el objeto HttpClient con los valores por defecto para poder realizar la autenticación del servicio
     * y envíandolo dentro del Header del HTTPMessageRequest.
     */
    private async Task<HttpClient> GetClient()
    {
      HttpClient client = new HttpClient();
      if (string.IsNullOrEmpty(_authorizationKey))
      {
        _authorizationKey = await client.GetStringAsync(Url + "login");
        _authorizationKey = _authorizationKey.Trim('"');
      }

      client.DefaultRequestHeaders.Add("Authorization", _authorizationKey);
      client.DefaultRequestHeaders.Add("Accept", "application/json");
      return client;
    }

    /*
     * Obtener toda la lista de libros que se encuentra en la base de datos.
     */
    public async Task<IEnumerable<Book>> GetAll()
    {
      // TODO: use GET to retrieve books
      HttpClient client = await GetClient();
      string result = await client.GetStringAsync(Url);
      return JsonConvert.DeserializeObject<IEnumerable<Book>>(result);
    }

    public async Task<Book> GetOne(string isbn)
    {
      // TODO: use GET to retrieve books
      HttpClient client = await GetClient();
      string result = await client.GetStringAsync(Url+isbn);
      
      return JsonConvert.DeserializeObject<Book>(result);
    }

    public async Task<Book> Add(string title, string author, string genre)
    {
      // TODO: use POST to add a book
      Book book = new Book()
      {
        Title = title,
        Authors = new List<string>(new[] { author }),
        ISBN = string.Empty,
        Genre = genre,
        PublishDate = DateTime.Now.Date,
      };
      HttpClient client = await GetClient();
      var response = await client.PostAsync(Url,
          new StringContent(
              JsonConvert.SerializeObject(book),
              Encoding.UTF8, "application/json"));

      return JsonConvert.DeserializeObject<Book>(
          await response.Content.ReadAsStringAsync());
    }

    public async Task<Book> Update(Book book)
    {

      HttpClient client = await GetClient();
      var response = await client.PutAsync(Url,
          new StringContent(
              JsonConvert.SerializeObject(book),
              Encoding.UTF8, "application/json"));

      return JsonConvert.DeserializeObject<Book>(
          await response.Content.ReadAsStringAsync());

    }

    public async Task<HttpClient> Delete(string isbn)
    {
      HttpClient client = await GetClient();
      var response = await client.DeleteAsync(Url + isbn);

      return client;
    }
  }
}

