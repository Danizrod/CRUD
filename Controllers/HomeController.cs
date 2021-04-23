using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CRUD.Negocio;

namespace CRUD.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using (var proced = new Negocio.Negocio())
            {
                var list = proced.GetAllObj();

                List<Obj> listViewModel = new List<Obj>();

                foreach (var item in list.Result)
                {
                    Obj objViewModel = new Obj()
                    {
                        Id = item.Id,
                        Prop1 = item.Props.Prop1,
                        Prop2 = item.Props.Prop2
                    };

                    listViewModel.Add(objViewModel);
                }
                return View(listViewModel);
            }
        }
        public IActionResult CreateDataObject(Obj obj)
        {
            if (obj != null)
            {

                var objDTO = new DTO.Obj
                {
                    Props = new DTO.Obj2()
                    {
                        Prop1 = obj.Prop1,
                        Prop2 = obj.Prop2
                    }
                };

                using (var proced = new Negocio.Negocio())
                {
                    var objUpdated = proced.CreateDataObject(objDTO);
                    if (objUpdated.Success)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            throw new Exception("Selecione um objeto válido!");
        }
        public IActionResult UpdateDataObject(Obj obj)
        {
            if (obj != null)
            {
                var objDTO = new DTO.Obj
                {
                    Id = obj.Id,
                    Props = new DTO.Obj2()
                    {
                        Prop1 = obj.Prop1,
                        Prop2 = obj.Prop2
                    }
                };

                using (var proced = new Negocio.Negocio())
                {
                    var objUpdated = proced.UpdateDataObject(objDTO);
                    if (objUpdated.Success)
                    {
                        return new JsonResult("ok");
                    }
                    else
                    {
                        return new JsonResult("Erro ao atualizar objeto!");
                    }
                }
            }
            throw new Exception("Selecione um objeto válido!");
        }
        public IActionResult DeleteDataObject(int id)
        {
            if (id != 0)
            {
                using (var proced = new Negocio.Negocio())
                {
                    var currentObj = proced.DeleteDataObject(id);
                    if (currentObj.Success)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            throw new Exception("Selecione um objeto válido!");
        }
        #region Métodos Auxiliares
        public class OperationResult<T> : OperationResult
        {
            public T Result { get; set; }
        }
        public class OperationResult
        {
            public string Message { get; set; }
            public bool Success { get; set; }
        }
        #endregion
    }
}
