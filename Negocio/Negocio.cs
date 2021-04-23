using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUD.Repository;
using static CRUD.Controllers.HomeController;
using CRUD.DTO;

namespace CRUD.Negocio
{
    public class NegocioBase : IDisposable
    {
        protected Repository.Repository _db;
        public NegocioBase()
        {
            _db = new Repository.Repository();
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
    public class Negocio : NegocioBase
    {
        private new Repository.Repository _db;

        public Negocio()
        {
            _db = new Repository.Repository();
        }

        #region Operations
        public OperationResult<List<Obj>> GetAllObj()
        {
            try
            {
                var objsList = _db.Obj.GetAll().ToList();

                if (objsList != null)
                {
                    List<Obj> list = new List<Obj>();
                    foreach (var item in objsList)
                    {
                        var objDTO = new Obj
                        {
                            Id = item.Id,
                            Props = new Obj2()
                            {
                                Prop1 = item.Prop1,
                                Prop2 = item.Prop2,
                            }
                        };
                        list.Add(objDTO);
                    }
                    return new OperationResult<List<Obj>>() { Message = "Operação realizada", Success = true, Result = list };
                }
                else
                {
                    return new OperationResult<List<Obj>>() { Message = "Não foi possivel localizar os objetos", Success = false };
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<List<Obj>>() { Message = ex.Message, Success = false };
            }
        }
        public OperationResult<Obj> UpdateDataObject(Obj obj)
        {
            try
            {
                var authenticatedObj = _db.Obj.Get(u => u.Id == obj.Id).FirstOrDefault();

                authenticatedObj.Prop1 = obj.Props.Prop1;
                authenticatedObj.Prop2 = obj.Props.Prop2;

                _db.Commit();

                return new OperationResult<Obj>() { Message = "Objeto alterado com sucesso!", Success = true };
            }
            catch (Exception ex)
            {
                return new OperationResult<Obj>() { Message = ex.Message, Success = false };
            }
        }
        public OperationResult<Obj> CreateDataObject(Obj obj)
        {
            try
            {
                var objEntity = new Database.Obj()
                {
                   Prop1 = obj.Props.Prop1,
                   Prop2 = obj.Props.Prop2
                };
                _db.Obj.Add(objEntity);
                _db.Obj.Save();

                return new OperationResult<Obj>() { Message = "Objeto adicionado com sucesso!", Success = true };

            }
            catch (Exception ex)
            {
                return new OperationResult<Obj>() { Message = ex.Message, Success = false };
            }
        }
        public OperationResult<Obj> DeleteDataObject(int objID)
        {
            try
            {
                var currentObj = _db.Obj.Get(x => x.Id == objID).FirstOrDefault();
                if (currentObj != null)
                {
                    _db.Obj.Delete(currentObj);

                    _db.Commit();

                    return new OperationResult<Obj>() { Message = "Objeto apagado com sucesso!", Success = true };
                }
                else
                {
                    return new OperationResult<Obj>() { Message = "Não foi possivel excluir o objeto", Success = true };
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<Obj>() { Message = ex.Message, Success = false };
            }

        }
        #endregion
    }
}
