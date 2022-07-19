using ProgressiveWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;

namespace ProgressiveWork.Controllers
{
    public class HomeController : Controller
    {

        string gblConnectionString = ConfigurationManager.ConnectionStrings["DBContext"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveData(EmployeeDto employeeM)
        {
            string sReturnValue = "OK";
            List<EmployeeDto> employees = null;
            try
            {
                employees = GetEmployees(employeeM);
            }
            catch(Exception ex)
            {
                sReturnValue = ex.Message;
            }
            return Json(new { Result = sReturnValue, Date = employees }, JsonRequestBehavior.AllowGet);
        }

        public List<EmployeeDto> GetEmployees(EmployeeDto employeeM)
        {
            
            List<EmployeeDto> employees = new List<EmployeeDto>();
            EmployeeDto employee = null;
            SqlConnection SqlConnection = new SqlConnection();
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataReader dr;
            try
            {
                //Get Connection
                SqlConnection.ConnectionString = gblConnectionString;
                SqlCommand.Connection = SqlConnection;
                SqlConnection.Open();

                //Sql query
                SqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                SqlCommand.CommandText = "SP_getEmployees";
                SqlCommand.Parameters.AddWithValue("@EmployeeName", employeeM.Name);
                SqlCommand.Parameters.AddWithValue("@EmployeeAddress", employeeM.Address);
                SqlCommand.Parameters.AddWithValue("@EmployeePhoneNumber", employeeM.Number);

                dr = SqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    employee = new EmployeeDto();
                    employee.Name = (dr["EmployeeName"] != DBNull.Value) ? Convert.ToString(dr["EmployeeName"]) : "";
                    employee.Address = (dr["EmployeeAddress"] != DBNull.Value) ? Convert.ToString(dr["EmployeeAddress"]) : "";
                    employee.Number = (dr["EmployeePhoneNumber"] != DBNull.Value) ? Convert.ToString(dr["EmployeePhoneNumber"]) : "";
                    employees.Add(employee);
                }

                dr.Close();
                SqlCommand.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnection.Close();
                SqlConnection.Dispose();
                SqlCommand.Dispose();

                SqlConnection = null;
                SqlCommand = null;
                dr = null;
            }
            return employees;
        }


        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}