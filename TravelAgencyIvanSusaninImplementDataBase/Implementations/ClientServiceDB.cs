using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninModel;

namespace TravelAgencyIvanSusaninImplementDataBase.Implementations
{
    public class ClientServiceDB : IClientService
    {
        private AbstractDbContext context;

        public ClientServiceDB(AbstractDbContext context) { this.context = context; }

        public List<ClientViewModel> GetList()
        {
            List<ClientViewModel> result = context.Clients.Select(rec => new ClientViewModel
            {
                Id = rec.Id,
                FIO = rec.FIO,
                Email = rec.Email,
                Login = rec.Login,
                Password = rec.Password
            })
            .ToList();
            return result;
        }

        public ClientViewModel GetElement(int id)
        {
            Client element = context.Clients.FirstOrDefault(rec => rec.Id == id); if (element != null)
            {
                return new ClientViewModel
                {
                    Id = element.Id,
                    FIO = element.FIO,
                    Email = element.Email,
                    Login = element.Login,
                    Password = element.Password
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ClientBindingModel model)
        {
            Client element = context.Clients.FirstOrDefault(rec => rec.FIO == model.FIO);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context.Clients.Add(new Client
            {
                FIO = model.FIO,
                Email = model.Email,
                Login = model.Login,
                Password = model.Password

            });
            context.SaveChanges();
        }

        public void UpdElement(ClientBindingModel model)
        {
            Client element = context.Clients.FirstOrDefault(rec => rec.FIO == model.FIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.FIO = model.FIO;
            element.Email = model.Email;
            element.Login = model.Login;
            element.Password = model.Password;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Client element = context.Clients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Clients.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
