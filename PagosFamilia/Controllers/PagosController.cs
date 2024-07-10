using PagosFamilia.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PagosFamilia.Controllers
{
    public class PagosController : Controller
    {

        IEnumerable<pagos> Pagos()
        {
            List<pagos> pagos = new List<pagos>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("sp_listar_compras", cn);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    pagos.Add(new pagos
                    {
                        id= dr.GetInt32(0),
                        monto= dr.GetDecimal(1),
                        fecha= dr.GetDateTime(2),

                    });
                }
                dr.Close();
                cn.Close();
            }
            return pagos;
            
        }
        // GET: Pagos
        public ActionResult Index()
        {
            return View(Pagos());
        }

        // GET: Pagos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Pagos/Create
        public ActionResult Create()
        {
            return View(new pagos());
        }

        // POST: Pagos/Create
        [HttpPost]
        public ActionResult Create(pagos pagos)
        {
            if (ModelState.IsValid)
            {
                    pagos.fecha= DateTime.Now;
                    using (SqlConnection cn= new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
                    {
                        using (SqlCommand cmd= new SqlCommand("usp_agregarCompra", cn))
                        {
                            cmd.CommandType= System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Monto", pagos.monto);
                            cmd.Parameters.AddWithValue("@Fecha", pagos.fecha);

                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            
                        }
                    }
                }
            ViewBag.Mensaje = "se agrego correctamente el pago";
            return View(pagos);

        }

        // GET: Pagos/Edit/5
        public ActionResult Edit(int id)
        {
            pagos pago = null;

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM compras WHERE id = @Id", cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pago = new pagos
                            {
                                id = (int)reader["id"],
                                monto = (decimal)reader["monto"],
                                fecha = (DateTime)reader["fecha"]
                            };
                        }
                    }

                    cn.Close();
                }
            }
            
            return View(pago);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(pagos pagos)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_editarCompra", cn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", pagos.id);
                        cmd.Parameters.AddWithValue("@Monto", pagos.monto);
                        cmd.Parameters.AddWithValue("@Fecha", pagos.fecha);

                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                        
                    }
                }

                ViewBag.Mensaje = "se ha editado correctamente el pago";
            }

            return View(pagos);
        }

        // GET: Pagos/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_eliminarCompra", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    
                }
            }
            
            return RedirectToAction("Index");

        }

        // POST: Pagos/Delete/5
       
    }
}
