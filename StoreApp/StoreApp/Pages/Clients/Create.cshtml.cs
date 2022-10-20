using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace StoreApp.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errormessage = "";
        public string successmessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
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
            //save the new client into the database
            try
            {
                string connectionstring = "Data Source=SOHAD-PC;Initial Catalog=StoreApp;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string CommandText = @"INSERT INTO [dbo].[Clients]
                                               ([Name]
                                               ,[Email]
                                               ,[Phone]
                                               ,[Address]
                                               ,[CreatedAt])
                                         VALUES
                                               (@name
                                               ,@email
                                               ,@phone
                                               ,@address" ;
                    using (SqlCommand command = new SqlCommand(CommandText, conn))
                    {
                        command.Parameters.AddWithValue("@Name", clientInfo.name);
                        command.Parameters.AddWithValue("@Email", clientInfo.email);
                        command.Parameters.AddWithValue("@Phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return;
            }
            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address = "";
            successmessage = "Successfuly Added.";
            Response.Redirect("/Clients/Index");
        }
    }
}
