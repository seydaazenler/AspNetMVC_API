using AspNetMVC_API_BLL.Repository;
using AspNetMVC_API_Entity.Models;
using System;
using System.Collections.Generic;
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
                    return "Kayıt başarılı olarak eklendi. id=" +newStudent.Id;
                    //ikinci yöntem
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
    }
}
