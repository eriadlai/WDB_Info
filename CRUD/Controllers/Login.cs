using CRUD.Data;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;

namespace CRUD.Controllers
{
    public class Login : Controller
    {
        private static string pathXML = @"C:\Users\eri_a\source\repos\CRUD\FirstXML.xml";
        private readonly ApplicationDbContext _context;
        public Login(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("usuario") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(string User, string Pass)
        {
            try
            {
                IEnumerable<Usuario> listUsuario = _context.Usuario;

                foreach (var item in listUsuario.ToArray())
                {
                    if (item.email.Equals(User.Trim()) && item.password.Equals(Pass.Trim()))
                    {
                        HttpContext.Session.SetString("usuario", item.email);
                        HttpContext.Session.SetString("rol", item.profesion);

                    }

                }


                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
        public ActionResult Perfil()
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewModels modelo = new ViewModels();
            modelo.listaUsuarios = _context.Usuario.Where(usu => usu.email.Equals(HttpContext.Session.GetString("usuario")));
            modelo.Sesion = HttpContext.Session.GetString("usuario");
            modelo.rol = HttpContext.Session.GetString("rol");
            return View(modelo);
        }
        public ActionResult XML()
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //===
            string pathCer = @"C:\Users\eri_a\source\repos\CRUD\CRUD\cañf770131pa3.cer";
            string pathKey = @"C:\Users\eri_a\source\repos\CRUD\CRUD\Claveprivada_FIEL_CAÑF770131PA3_20190614_170345.key";
            string ClavePrivada = "12345678a";
            //OBTENER NUMERO====
            string numeroCertificado, aa, bb, c;
            SelloDigital.leerCER(pathCer, out aa, out bb, out c, out numeroCertificado);




            //==============================================
            Comprobante oComprobante = new Comprobante();
            oComprobante.Version = "3.3";
            oComprobante.Serie = "H";
            oComprobante.Folio = "123";
            oComprobante.Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            oComprobante.FormaPago = "99";
            oComprobante.NoCertificado = numeroCertificado;
            oComprobante.SubTotal = 10m;
            oComprobante.Descuento = 1;
            oComprobante.Moneda = "MXN";
            oComprobante.Total = 9;
            oComprobante.TipoDeComprobante = "I";
            oComprobante.MetodoPago = "PUE";
            oComprobante.LugarExpedicion = "20131";


            ComprobanteEmisor oEmisor = new ComprobanteEmisor();
            oEmisor.Rfc = "XIQB891116QE4";
            oEmisor.Nombre = "RAZON";
            oEmisor.RegimenFiscal = "605";


            ComprobanteReceptor oReceptor = new ComprobanteReceptor();
            oReceptor.Nombre = "NOMBRE PERSONA";
            oReceptor.Rfc = "CACX7605101P8";
            oReceptor.UsoCFDI = "P01";

            //ASIGNACION DE EMISOR Y RECEPTOR A COMPROBANTE====
            oComprobante.Emisor = oEmisor;
            oComprobante.Receptor = oReceptor;

            List<ComprobanteConcepto> listConcepto = new List<ComprobanteConcepto>();
            ComprobanteConcepto oConcepto = new ComprobanteConcepto();
            oConcepto.Importe = 10m;
            oConcepto.ClaveProdServ = "92111704";
            oConcepto.Cantidad = 1;
            oConcepto.ClaveUnidad = "C81";
            oConcepto.Descripcion = "DESCRIPCION DEL PRODUCTO";
            oConcepto.ValorUnitario = 10m;
            oConcepto.Descuento = 1;

            listConcepto.Add(oConcepto);

            oComprobante.Conceptos = listConcepto.ToArray();


            CreateXML(oComprobante);
           

            string cadenaOriginal = "";
            string pathXslt = @"C:\Users\eri_a\source\repos\CRUD\CRUD\cadenaoriginal_TFD_1_1.xslt";
            System.Xml.Xsl.XslCompiledTransform transformador = new System.Xml.Xsl.XslCompiledTransform(true);
            transformador.Load(pathXslt);
            
            using (StringWriter sw = new StringWriter())
            using (XmlWriter xwo = XmlWriter.Create(sw, transformador.OutputSettings))
            {
                transformador.Transform(pathXML,xwo);
                cadenaOriginal = sw.ToString();
            }

            SelloDigital oSelloDigital = new SelloDigital();
            oComprobante.Certificado = oSelloDigital.Certificado(pathCer);
            oComprobante.Sello = oSelloDigital.Sellar(cadenaOriginal,pathKey,ClavePrivada);

            CreateXML(oComprobante);

            //TIMBRE====
            //ServiceReferenceFC.RespuestaCFDI respuestaCFDI = new ServiceReferenceFC.RespuestaCFDI();
            //byte[] bxml = System.IO.File.ReadAllBytes(pathXML);
            //ServiceReferenceFC.TimbradoClient oTimbrado = new ServiceReferenceFC.TimbradoClient();
            //respuestaCFDI = oTimbrado.TimbrarTest("Test","paswordTest",bxml);
            //if(respuestaCFDI.Documento==null){
            //Console.WriteLine(respuestaCFDI.Mensaje);
            //}else{
            //System.IO.File.WriteAllBytes(pathXML,respuestaCFDI.Documento);
            //}

                return RedirectToAction("Index", "Home");
        }

        private static void CreateXML(Comprobante oComprobante) {

            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add("cfdi", "http://www.sat.gob.mx/cfd/3");
            xmlNamespaces.Add("tdf", "http://www.sat.gob.mx/TimbreFiscalDigital");
            xmlNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            XmlSerializer oSerializer = new XmlSerializer(typeof(Comprobante));
            string Sxml = "";
            using (var sww = new Data.StringWriterWithEncoding(Encoding.UTF8))
            {
                using (XmlWriter writter = XmlWriter.Create(sww))
                {
                    oSerializer.Serialize(writter, oComprobante,xmlNamespaces);
                    Sxml = sww.ToString();
                }
            }
            System.IO.File.WriteAllText(pathXML, Sxml);
        }
        
    }
}
