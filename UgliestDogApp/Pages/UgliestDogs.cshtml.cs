 using Microsoft.AspNetCore.Mvc;
   using Microsoft.AspNetCore.Mvc.RazorPages;
   using Microsoft.AspNetCore.Mvc.Rendering;
   using Microsoft.Data.Sqlite;
   using System.Collections.Generic;
// using the ugliestdog model to the page model, using the list from the previous page. 
   public class UgliestDogsModel : PageModel
   {
       public List<SelectListItem> DogList { get; set; }
       public Dog SelectedDog { get; set; }
// it will detect the page up through this OnGet.
       public void OnGet()
       {
           LoadDogList();
       }
// it will choose a selected dog from the selectedDog method. 
       public void OnPost(string selectedDog)
       {
           LoadDogList();
           if (!string.IsNullOrEmpty(selectedDog))
           {
               SelectedDog = GetDogById(int.Parse(selectedDog));
           }
       }
// this private void will provide all of the loaded dogs in this list and create a connection. 
       private void LoadDogList()
       {
           DogList = new List<SelectListItem>(); // it will likely connect to the sqlite .db
           using (var connection = new SqliteConnection("Data Source=UgliestDogs.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT Id, Name FROM Dogs";
               using (var reader = command.ExecuteReader())
               {
                   while (reader.Read())
                   { // this will get the int to each dog. 
                       DogList.Add(new SelectListItem
                       {
                           Value = reader.GetInt32(0).ToString(),
                           Text = reader.GetString(1)
                       });
                   }
               }
           }
       }
// this will get the id from the sqlite database and reads out the information. 
       private Dog GetDogById(int id)
       {
           using (var connection = new SqliteConnection("Data Source=UgliestDogs.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT * FROM Dogs WHERE Id = @Id";
               command.Parameters.AddWithValue("@Id", id);
               using (var reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       return new Dog
                       { // by using this list below
                           Id = reader.GetInt32(0),
                           Name = reader.GetString(1),
                           Breed = reader.GetString(2),
                           Year = reader.GetInt32(3),
                           ImageFileName = reader.GetString(4)
                       };
                   }
               }
           }
           return null;
       }
   }
// this is storing the information for all the dogs selected. 
   public class Dog
   {
       public int Id { get; set; }
       public string Name { get; set; }
       public string Breed { get; set; }
       public int Year { get; set; }
       public string ImageFileName { get; set; }
   }
