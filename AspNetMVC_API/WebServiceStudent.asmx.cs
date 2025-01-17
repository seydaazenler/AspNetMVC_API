﻿using AspNetMVC_API_BLL.Repository;
using AspNetMVC_API_Entity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AspNetMVC_API
{
    
    /// <summary>
    /// Summary description for WebServiceStudent
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceStudent : System.Web.Services.WebService
    {
        //Global Alan
        StudentRepo myStudentRepo = new StudentRepo();
        private bool IsAuthenticated
        {
            get
            {
                bool result = false;
                try
                {
                    string authorization = "";
                    authorization = HttpContext.Current.Request.Headers["Authorization"];
                    if (authorization!=null)
                    {
                        authorization = authorization.Replace("Basic", "");
                        byte[] byteArray = Convert.FromBase64String(authorization);
                        string usernamePassword = System.Text.Encoding.UTF8.GetString(byteArray);

                        //betul:12345
                        bool usernameResult = usernamePassword.Split(':')
                            .First().Equals(ConfigurationManager.AppSettings["USERNAME"].ToString());

                        bool passwordResult = usernamePassword.Split(':')
                            .Last().Equals(ConfigurationManager.AppSettings["PASSWORD"].ToString());

                        //username ve parola doğru geldiyse kontrol etsin result'a true atsın. Doğru değilse false atsın.
                        result = (usernameResult && passwordResult) ? true : false;
                    }
                    return result;
                }
                catch (Exception)
                {
                    result = false;
                    return result;
                }
            }
        }

        private void CheckCredentials()
        {
            if (!IsAuthenticated)
            {
                throw new Exception("Kullanıcı adı ve şifre hatalıdır! Tekrar deneyiniz.");
            }
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public List<Student> GetAll()
        {
            try
            {
                CheckCredentials();
                List<Student> list = myStudentRepo.GetAll();
                return list;
            }
            catch (Exception ex)
            {
                //loglama yapılabilir
                return null;
            }
        }

        [WebMethod]

        public string Insert(string name, string surname)
        {
            try
            {
                CheckCredentials();
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname))
                {
                    throw new Exception("(name-surname) alanları boş bırakılamaz!");
                }
                Student newStudent = new Student()
                {
                    Name=name,
                    Surname=surname
                };
                int insertResult = myStudentRepo.Insert(newStudent);
                if (insertResult>0)
                {
                    //birinci yöntem
                    //return "Kayıt başarılı olarak eklendi. id=" +newStudent.Id;
                    //ikinci yöntem
                    string jsonString = JsonConvert.SerializeObject(newStudent);
                    return jsonString;
                }
                else
                {
                    throw new Exception("Kayıt ekleme işleminde beklenmedik bir hata oluştu!");
                }
            }
            catch (Exception ex)
            {
                //string ile dönüş olduğu için ex.message gödneririz
                return ex.Message;
            }
        }

        [WebMethod]
        public string Delete(int id)
        {
            try
            {
                CheckCredentials();
                if (id>0)
                {
                    Student student = myStudentRepo.GetById(id);
                    if (student == null)
                    {
                        throw new Exception("Öğrenci bulunamadığı için silme işlemi başarısız!");
                    }
                    int deleteResult = myStudentRepo.Delete(student);
                    if (deleteResult>0)
                    {
                        return "Kayıt başarıyla silindi!";
                    }
                    else
                    {
                        throw new Exception("Beklenmedik bir hata oluştuğu için kayıt silinemedi!");
                     }
                }
                else
                {
                    throw new Exception("Gönderilen id değeri sıfırdan büyük olmalıdır!");
                }

            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        [WebMethod]
        public string Update(int currentid, string newname, string newsurname)
        {
            try
            {
                CheckCredentials();
                if (currentid<=0)
                {
                    throw new Exception("Gönderilen id değeri sıfırdan büyük olmalıdır!");
                }
                if (string.IsNullOrEmpty(newname) && string.IsNullOrEmpty(newsurname))
                {
                    throw new Exception("(newname-newsurname) alanları boş boırakılamaz!");
                }
                Student currentStudent = myStudentRepo.GetById(currentid);
                if (currentStudent==null)
                {
                    throw new Exception("Öğrenci bulunamadığı için güncelleme işlemi başarısızdır!");
                }
                //eğer newname parametresi dolu ise isim güncellenecek
                if (!string.IsNullOrEmpty(newname))
                {
                    currentStudent.Name = newname;
                }

                //eğer newsurname parametresi dolu ise soyisim güncellenecek
                if (!string.IsNullOrEmpty(newsurname))
                {
                    currentStudent.Surname = newsurname;
                }
                int updateResult = myStudentRepo.Update();
                if (updateResult>0)
                {
                    return "Güncelleme işlemi başarıyla gerçekleşti";
                }
                else
                {
                    throw new Exception("Beklenmedik bir hata nedeniyle kayıt güncellenemedi");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
