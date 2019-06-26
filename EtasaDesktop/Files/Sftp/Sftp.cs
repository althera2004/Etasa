using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EtasaDesktop.Files.Sftp
{
    class Sftp
    {
        public Sftp()
        {

        }
        //metodo de descarga de los ficheros SFTP 
        public void DownloadListFilesDirectory(string host, string username, string password, int port, string LocalFolder)
        {
            //PerCent.Text = "Empezar a descargar";
            string name = "";
            int numberfiles;
            //comprobamos que el directio actual existe
            if (Directory.Exists(LocalFolder))
            {
                using (var sftp = new SftpClient(host, port, username, password))
                {
                    try
                    {
                        //realizamos la connexión
                        sftp.Connect();
                        //obtenemos el listado de ficheros 
                        var files2 = sftp.ListDirectory(sftp.WorkingDirectory + "Dat");
                        //comprobamos que hay ficheros a descargar en el servidor remos 
                        numberfiles = files2.Count();
                        if (numberfiles > 0)
                        {
                            //CaluculateAll(progressBar1, 100);
                            //recorremos las rutar de los ficheros remotos 
                            foreach (var file in files2)
                            {
                                name = file.Name;
                                if (name != "." && name != ".." && !string.IsNullOrEmpty(name))
                                {
                                    //creamos un objeto vacio en la ruta donde gravaremos el fichero el cual apunta a la carpeta local
                                    using (Stream fileStream = File.Create(LocalFolder + @"\" + name))
                                    {
                                        //accedemos a los ficheros del servidor y lo guardamos en el fihcero creado con anterioridad pero en la carpeta local 
                                        sftp.DownloadFile("Dat/" + name, fileStream);
                                    }
                                }
                            }
                            /*
                            //recorremos al carpeta del servidor remoto para eliminar los archivos
                            foreach (var file in files2)
                            {
                                //obtenemos el nombre del fichero 
                                name = file.Name;
                                if (name != "." && name != ".." && !string.IsNullOrEmpty(name))
                                {
                                    //borramos todos los ficheros de la carpeta dat
                                    sftp.DeleteFile("Dat/" + name);
                                }
                            }
                            */
                            //PerCent.Text = "Files Upload Corretly";

                        }
                        else
                        {
                            MessageBox.Show("En el directorio no hay fichero para descargar");
                            //PerCent.Text = "Files Upload Error";
                        }
                    }
                    catch (Renci.SshNet.Common.SftpPermissionDeniedException e)
                    {
                        string excepcion = e.ToString();
                        //PerCent.Text = "Files Upload Error";
                    }
                    //desconectamos la conexión   
                    sftp.Disconnect();
                }
            }
            else
            {
                MessageBox.Show("El directorio local no existe");
                //PerCent.Text = "Files Upload Error";
            }
        }

        //subida de fichero al servidor SFTp
        public void UploadListFilesDirectory(string host, string username, string password, int port, string remoteDirectorydat, string LocalFolder)
        {
            //PerCent.Text = "Empezar a descargar";
            string name = "";
            int numberfiles;
            //sftp.CreateDirectory(SFTPFolderDest + FolderName);//Create folder if necessary else skip

            //miramos que el directorio local exista
            if (Directory.Exists(LocalFolder))
            {
                //obetenemos la lista de rutas de ficheros de la carpeta local 
                var files = Directory.GetFiles(LocalFolder);
                //comrprovamos que hay ficheros para subir 
                numberfiles = files.Count();
                //si en la carpeta hay ficheros realizamos la conexión y el proceso de transmisión
                if (numberfiles != 0)
                {
                    using (var sftp = new SftpClient(host, port, username, password))
                    {
                        try
                        {
                            //realizamos la connexión 
                            sftp.Connect();
                            //CaluculateAll(progressBar1, 100);
                            //recorremos la lista para insertar los archivos en la carpeta tmp
                            foreach (var file in files)
                            {
                                //obtenemos el nombre del fichero con extension 
                                name = System.IO.Path.GetFileName(file);
                                using (var fileStream = new FileStream(file, FileMode.Open))
                                {
                                    // bypass Payload error large files
                                    sftp.BufferSize = 4 * 1024;
                                    //se sube a la ruta configurada del servidor Sftp
                                    sftp.UploadFile(fileStream, "tmp/" + name);
                                }
                            }

                            //recorremos la lista de fichero insertados en la carpeta Sftp/Tmp y los movemos a la carpeta Dat
                            //obtenemos la lista de rutas de ficheros del servidor remoto
                            var files2 = sftp.ListDirectory(sftp.WorkingDirectory + "tmp");
                            foreach (var file in files2)
                            {
                                //obtenemos el nombre del fichero 
                                name = file.Name;
                                //si el fichero ya existe en la carpeta dat no lo pasamos
                                if (!sftp.Exists("Dat/" + name) && name != "." && name != ".." && !string.IsNullOrEmpty(name))
                                {
                                    //pasamos del servidor remoto carpeta tmp a la carpeta Dat
                                    sftp.RenameFile("tmp/" + name, "Dat/" + name);
                                }
                            }
                            //PerCent.Text = "Files Download Correctly";
                            //desconectamos la conexión
                            sftp.Disconnect();
                        }
                        catch (Renci.SshNet.Common.SftpPermissionDeniedException e)
                        {
                            string excepcion = e.ToString();
                            // PerCent.Text = "Files Download Error";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("En el directorio no hay fichero para transmitir");
                    //PerCent.Text = "Files Download Error";
                }
            }
            else
            {
                MessageBox.Show("El directorio local utilizado no existe");
                //PerCent.Text = "Files Download Error";
            }
        }
    }
}
