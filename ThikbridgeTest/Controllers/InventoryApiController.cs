using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ThikbridgeTest;
using ThikbridgeTest.Models;

namespace ThikbridgeTest.Controllers
{
    public class InventoryApiController : ApiController
    {
        public readonly PetaPoco.Database db=null;
        public InventoryApiController()
        {
            db= new PetaPoco.Database("cnstr");
        }

        // GET: api/InventoryApi
        public List<Component> GetComponentList()
        {            
            return db.Fetch<Component>("Select * from Component").ToList();
        }

        public void InsertComponent(Component component)
        {
            db.Insert(component);

        }
       
        public Component GetComponentDetails(int Id)
        {
            var data= db.FirstOrDefault<Component>($"Select * from Component where Id={Id}");
            return data;
        }
        public int DeleteComponent(int Id)
        {
            db.Execute("Delete from Component where ID=@0", Id);
            return 0;
        }
    }
}