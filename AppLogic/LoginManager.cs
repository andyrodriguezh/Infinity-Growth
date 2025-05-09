using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLogic.Services;
using DataAccess.Crud;
using DTO;
using BCrypt.Net;


namespace AppLogic
{
    public interface ILoginManager
    {
        public List<Usuarios> GetUserLogin(string pCorreo, string pPassword);
        public Usuarios GetUserById(int pId);
        public bool ValidarOTP(int usuario, int otp);
    }
    public class LoginManager : ILoginManager
    {
        private readonly IEmailService _emailService;

        public LoginManager(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public List<Usuarios> GetUserLogin(string pCorreo, string pPassword) // Se obtiene el usuario que intenta iniciar sesión y se genera un OTP
        {
            var appCrud = new LoginCrud();
            var encrypt = new Encrypt_Service();
             
            var appList = appCrud.RetrieveByCorreo<Usuarios>(pCorreo);

            if (appList.Count > 0)
            {
                var user = appList[0];

                if (encrypt.VerifyPassword(pPassword, user.Password)) // Se verifica que la contraseña ingresada sea correcta   
                {
                    if (user.Estado == 0)
                    {
                        return new List<Usuarios>() { user };
                    }

                    if (user.Estado == 2)
                    {
                        return new List<Usuarios>() { user };
                    }

                    if (user.isPasswordTemp == true)
                    {
                        return new List<Usuarios> { user };
                    }
                    else
                    {
                        var OTP = new OTP_Service(_emailService);
                        string solicitud = "Inicio de sesión";
                        _ = OTP.GenerateOTP(user.Id, user.Correo, solicitud);

                        return new List<Usuarios> { user };
                    }
                }
            }

            return new List<Usuarios>();
        }

        public bool ValidarOTP(int pUsuario, int pOtp)
        {
            var otpServices = new OTP_Service(_emailService);

            return otpServices.ValidateOTP(pUsuario, pOtp);
        }

        public Usuarios GetUserById(int pId)
        {
            var appCrud = new LoginCrud();
            var appList = appCrud.RetrieveByUserId<Usuarios>(pId);

            if (appList.Count > 0)
            {
                return appList[0];
            }
            return new Usuarios();
        }


    }
}

