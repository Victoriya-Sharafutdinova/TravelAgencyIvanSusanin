using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgencyIvanSusaninDAL.ViewModel
{
    public class ClientViewModel
    {
        public int Id { get; set; }

        [DisplayName("ФИО клиента")]
        public string FIO { get; set; }

        [DisplayName("Почта клиента")]
        public string Email { get; set; }

        [DisplayName("Логин")]
        public string Login { get; set; }

        [DisplayName("Пароль")]
        public string Password { get; set; }

    }
}
