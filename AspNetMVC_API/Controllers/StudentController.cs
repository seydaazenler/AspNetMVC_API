using AspNetMVC_API.Models.ViewModels;
using AspNetMVC_API_BLL.Repository;
using AspNetMVC_API_Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;


namespace AspNetMVC_API.Controllers
{
    [System.Web.Http.RoutePrefix("ogrenci")]
    public class StudentController : ApiController
    {
        //Global Alan
        StudentRepo myStudentRepo = new StudentRepo();

        [System.Web.Http.Route("")]
        public ResponseData GetAll()
        {
            try
            {
                var list = myStudentRepo.GetAll().Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Surname,
                    x.RegisterDate
                }
                ).ToList();
                if (list != null)
                {
                    if (list.Count == 0)
                    {
                        return new ResponseData()
                        {
                            Success = true,
                            Message = "Sistemde kayıtlı öğrenci bulunmamaktadır",
                            Data = list
                        };
                    }
                    return new ResponseData()
                    {
                        Success = true,
                        Message = "Tüm öğrenciler listesi başarı ile gönderildi...",
                        Data = list

                    };
                }
                else
                {
                    return new ResponseData()
                    {
                        Success = false,
                        Message = "Tüm öğrencileri getirirken bir hata oluştu!"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseData()
                {
                    Success = false,
                    Message = ex.Message
                };

            }
        }

        //kendimiz route belirleyebiliriz böyle bir kod bloğu yazarsak
        //http://localhost:62911/detay/1 yazılarak id=1 olan kişinin detayları verir

        [System.Web.Http.Route("detay/{id:int:min(1)}")]
        public ResponseData GetDetail(int id)
        {
            try
            {
                var student = myStudentRepo.GetById(id);
                if (student == null)
                {
                    throw new Exception("Gönderilen IDye ait kayıt bulunamadı!");
                }
                return new ResponseData()
                {
                    Success = true,
                    Message = "Kayıt bulundu",
                    Data = new
                    {
                        student.Id,
                        student.Name,
                        student.Surname,
                        student.RegisterDate


                    }
                };
            }
            catch (Exception ex)
            {
                return new ResponseData()
                {
                    Success = true,
                    Message = ex.Message
                };
            }
        }

        //public ResponseData GetById(int id)
        //{
        //    try
        //    {
        //        if (id > 0)
        //        {
        //            var student = myStudentRepo.GetById(id);
        //            if (student == null)
        //            {
        //                throw new Exception($"{id} değerinde bir kayıt bulunamadı!");
        //            }
        //            return new ResponseData()
        //            {
        //                Success = true,
        //                Message = "Kayıt bulundu.",
        //                Data = new
        //                {
        //                    student.Id,
        //                    student.Name,
        //                    student.Surname,
        //                    student.RegisterDate

        //                }
        //            };
        //        }

        //        else
        //        {
        //            return new ResponseData()
        //            {
        //                Success = false,
        //                Message = "Negatif değer gönderilemez!"
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData()
        //        {
        //            Success = false,
        //            Message = ex.Message

        //        };

        //    }
        //}

        [System.Web.Http.HttpPost]
        public ResponseData Insert([FromBody]StudentViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return new ResponseData()
                    {
                        Success = false,
                        Message = "Ekleme işlemi için verilerinizi doğru şekilde göndermeniz gerekir!"
                    };
                }
                Student newStudent = new Student()
                {
                    Name = model.name,
                    Surname = model.surname,
                    RegisterDate = model.registerdate
                };
                if (myStudentRepo.Insert(newStudent) > 0)
                {
                    return new ResponseData()
                    {
                        Success = true,
                        Message = $"Yeni öğrenci eklendi, Id={newStudent.Id}"
                    };
                }
                else
                {
                    throw new Exception("Öğrenci ekleme işlemi başarısız");
                }
            }
            catch (Exception ex)
            {
                return new ResponseData()
                {
                    Success = false,
                    Message = ex.Message
                };

            }
        }
        [System.Web.Http.HttpPost]
        public ResponseData Update([FromBody]StudentViewModel model)
        {
            try
            {
                if (model==null)
                {
                    return new ResponseData()
                    {
                        Success = false,
                        Message = "Güncelleme yapabilmek için veri girişi gereklidir!"
                    };
                }
                var student = myStudentRepo.GetById(model.id);
                if (student==null)
                {
                    return new ResponseData()
                    {
                        Success = false,
                        Message = "Öğrenci bulunamadı. Güncelleme başarısız!"
                    };
                }

                student.Name = model.name;
                student.Surname = model.surname;
                if (myStudentRepo.Update()>0)
                {
                    return new ResponseData()
                    {
                        Success = true,
                        Message = "Güncelleme işlemi başarılıdır!"
                    };
                }
                else
                {
                    throw new Exception("Öğrenci güncelleme işlemi beklenmedik bir hata nedeniyle başarısız oldu!");
                }


            }
            catch (Exception ex)
            {
                return new ResponseData()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        [System.Web.Http.HttpPost]
        public ResponseData Delete(int id)
        {
            try
            {
                var student = myStudentRepo.GetById(id);
                if (student==null)
                {
                    throw new Exception("Gönderilen Id' ye ait öğrenci bulunamadı! Silme işlemi başarısız.");
                }
                if (myStudentRepo.Delete(student)>0)
                {
                    return new ResponseData()
                    {
                        Success = true,
                        Message = "Kayıt silindi"
                    };
                }
                else
                {
                    throw new Exception("Beklenmedik bir hata nedeniyle kayıt silinemedi!");
                    
                }
            }
            catch (Exception ex)
            {
                return new ResponseData()
                {
                    Success = false,
                    Message = ex.Message
                };
                
            }
        }

        
    }
}