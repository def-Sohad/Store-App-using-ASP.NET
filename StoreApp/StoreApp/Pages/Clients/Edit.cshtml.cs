using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace StoreApp.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errormessage = "";
        public string successmessage = "";
        public void OnGet()
        {
            string id = Request.Query["Id"];
            try
            {
                string connectionstring = "Data Source=SOHAD-PC;Initial Catalog=StoreApp;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string CommandText = "SELECT * FROM Clients WHERE Id=@id";
                    using (SqlCommand cmd = new SqlCommand(CommandText, conn))
                    {
                        cmd.Parameters.AddWithValue("@id",id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return;
            }
        }
        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0
                || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errormessage = "All Client Infromation are required!";
                return;
            }
            try
            {
                string connectionstring = "Data Source=SOHAD-PC;Initial Catalog=StoreApp;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string CommandText = @"UPDATE [dbo].[Clients]
                                           SET [Name] = @name
                                              ,[Email] = @email
                                              ,[Phone] = @phone
                                              ,[Address] = @address
                                         WHERE Id=@id";
                    using (SqlCommand command = new SqlCommand(CommandText, conn))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@id", clientInfo.id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return;
            }
            Response.Redirect("/Clients/Index");
        }
    }
}
