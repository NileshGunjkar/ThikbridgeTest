using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ThikbridgeTest.Models;

namespace ThikbridgeTest.Controllers
{
    public class InventoryController : Controller
    {

        static readonly string rootFolder ="~/Content/Images";
        static readonly string apiUrl = "https://localhost:44363/api/InventoryApi/";
        // GET: Inventory
        private static async Task<List<Component>> ComponentList()
        {
            List<Component> comList = new List<Component>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method  
                HttpResponseMessage response = await client.GetAsync("GetComponentList");
                if (response.IsSuccessStatusCode)
                {
                    comList = await response.Content.ReadAsAsync<List<Component>>();
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }

            return comList;
        }
        public async Task<ActionResult> Index()
        {
            ComponentViewModel componentViewModel = new ComponentViewModel();
            List<Component> comList = await ComponentList();
            componentViewModel.componentModelList = comList;
            return View(componentViewModel);
        }

        // GET: Inventory/Details/5
        public async Task<ActionResult> Details(int Id)
        {
            Component model = new Component();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method  
                HttpResponseMessage response = await client.GetAsync("GetComponentDetails?id=" + Id.ToString());
                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<Component>();
                    return View(model);
                }
            }
            return View(model);

        }

        // POST: Inventory/Create
        [HttpPost]
        public async Task<ActionResult> Create(HttpPostedFileBase file, Component model)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    model.Picture = file.FileName;
                    string path = Path.Combine(Server.MapPath(rootFolder), Path.GetFileName(file.FileName));
                    file.SaveAs(path);

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        var postTask = client.PostAsJsonAsync<Component>("InsertComponent", model);
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
                
            }
            ComponentViewModel componentViewModel = new ComponentViewModel();
            List<Component> comList = await ComponentList();
            componentViewModel.componentModelList = comList;
            componentViewModel.componentModel = model;
            return View("Index", componentViewModel);
        }

        public async Task<ActionResult> Delete(int Id)
        {
            Component model = new Component();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method  
                HttpResponseMessage response = await client.DeleteAsync("DeleteComponent?id=" + Id.ToString());
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
