using System.Net.Http.Headers;
using DispatchBalanceAPI.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DispatchBalanceAPI.Bussines
{
    public class BDProcess
    {
        public async Task<List<DispatchBalanceHeader>> getDataService(string s_date, int s_page, string s_ceve)
        {
            string? credentials = "";
            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("service_credential_usr")))
                return new List<DispatchBalanceHeader>();
            else
                credentials = Environment.GetEnvironmentVariable("service_credential_usr");

            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("service_credential_pwd")))
                return new List<DispatchBalanceHeader>();
            else
                credentials += ":" + Environment.GetEnvironmentVariable("service_credential_pwd");

            //Iniciar una llamada http al servicio de Existencias
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials)));

            //Manejar la respuesta http
            var response = await client.GetAsync(Environment.GetEnvironmentVariable("url_servicio_existencias") + "&date=" + s_date + "&page=" + s_page + "&ceve=" + s_ceve);

            //Convierte todos los parámetros de la solicitud en un objeto Json
            var json = await response.Content.ReadAsStringAsync();

            var listRoute = JsonConvert.DeserializeObject<List<DispatchBalanceHeader>>(json);

            return listRoute == null ? new List<DispatchBalanceHeader>() : listRoute; ;
        }

        public async Task<List<DispatchBalanceHeader>> getDispatchBalanceServices(DateTime saleDate, int ceveCode, DispatchBalanceContext _dispatchContext)
        {

            try
            { 
                   var balanceHeader = await _dispatchContext.dbHeader.Where(i => i.SaleDate == saleDate && i.CeveCode == ceveCode).ToListAsync();
            if (balanceHeader.Count > 0)
            {
                return balanceHeader;
            }
            else
            {
                return new List<DispatchBalanceHeader>(); 
            }
            }
            catch (Exception ex)
            {
                //MENSAJES PARA BITÁCORA
                //Console.WriteLine("Se produjo el siguiente error al hacer el Insert en la tabla destino: " + ex.Message);
                //if (ex.InnerException is not null)
                //    return "false " + "error al hacer el insert en BD " + ex.InnerException.Message;
                //else
                //    return "false " + "error al hacer el insert en BD " + ex.Message;
                return new List<DispatchBalanceHeader>();

            }
        }
    }
}
