using EtasaDesktop.Common.Data;
using EtasaDesktop.Distribution.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EtasaDesktop.Distribution.Planner
{

    public class PlannerRequester
    {
        public static ReadOnlyCollection<Factory> FactoryList;
        public static ReadOnlyCollection<Product> ProductList;
        public static ReadOnlyCollection<Operator> OperatorList;

        public static string GetColorPlanned()
        {
            PlannerDataSet ds = new PlannerDataSet();
            ds.EnforceConstraints = false;
            string colourPlanned = "";
            DataTable ColourPlanned = new DataTable();
            PlannerDataSetTableAdapters.StatusTableAdapter adapter = new PlannerDataSetTableAdapters.StatusTableAdapter();
            ColourPlanned = adapter.GetDataStatusById(2);
            colourPlanned = ColourPlanned.Rows[0][2].ToString();
            return colourPlanned; 
        }

        public static ReadOnlyCollection<Product> RequestProducts()
        {
            var res = new List<Product>();
            using (var cmd = new SqlCommand("Product_GetAll"))
            {
                using (var cnn = new SqlConnection(Properties.Settings.Default.EtasaConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newProduct = new Product
                                {
                                    Id = rdr.GetInt32(0),
                                    Name = rdr.GetString(1),
                                    Code = rdr.GetString(2),
                                    Density = rdr.GetDecimal(5),
                                    MeasureUnit = rdr.GetInt16(6),
                                    Enabled = rdr.GetBoolean(7)
                                };

                                res.Add(newProduct);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var x = ex.Message;
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            ProductList = new ReadOnlyCollection<Product>(res);
            return ProductList;
        }

        public static ReadOnlyCollection<Factory> RequestFactories()
        {
            var res = new List<Factory>();
            using (var cmd = new SqlCommand("Factory_GetAll"))
            {
                using (var cnn = new SqlConnection(Properties.Settings.Default.EtasaConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newFactory = new Factory
                                {
                                    Id = rdr.GetInt32(3),
                                    Name = rdr.GetString(0),
                                    Code = rdr.GetString(2),

                                    Location = new LocationData
                                    {
                                        Address = rdr.GetString(5),
                                        City = rdr.GetString(6),
                                        PostCode = rdr.GetString(7),
                                        Province = rdr.GetString(8),
                                        Country = rdr.GetString(9),
                                        Latitude = rdr.GetFloat(10),
                                        Longitude = rdr.GetFloat(11)
                                    },
                                    Enabled = rdr.GetBoolean(12),
                                    Observations = rdr.GetString(13),
                                    HexColor = rdr.GetString(14),
                                    Colors = new Common.Data.FactoryColors
                                    {
                                        Factory = rdr.GetString(14),
                                        Cliente = rdr.GetString(15),
                                        Urgent = rdr.GetString(16),
                                        Preference = rdr.GetString(17),
                                        LastPreferent = rdr.GetString(18)
                                    }
                                };

                                res.Add(newFactory);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var x = ex.Message;
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            FactoryList = new ReadOnlyCollection<Factory>(res);
            return FactoryList;
        }

        public static ReadOnlyCollection<Operator> RequestOperators()
        {
            var res = new List<Operator>();
            using (var cmd = new SqlCommand("Operator_GetAll"))
            {
                using (var cnn = new SqlConnection(Properties.Settings.Default.EtasaConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newOperator = new Operator
                                {
                                    Id = rdr.GetInt32(0),
                                    Code = rdr.GetString(1),
                                    Name = rdr.GetString(2),
                                    Cif = rdr.GetString(3),
                                    Location = new LocationData
                                    {
                                        Address = rdr.GetString(4),
                                        City = rdr.GetString(5),
                                        PostCode = rdr.GetString(6),
                                        Province = rdr.GetString(7),
                                        Country = rdr.GetString(8)
                                    },
                                    Contact = new ContactData
                                    {
                                         Name = rdr.GetString(9),
                                         Email = rdr.GetString(10)
                                    },
                                    Enabled = rdr.GetBoolean(11),
                                    Observations = rdr.GetString(12)
                                };

                                res.Add(newOperator);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var x = ex.Message;
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            OperatorList = new ReadOnlyCollection<Operator>(res);
            return OperatorList;
        }

        public static ObservableCollection<Order> RequestOrders(DateTime fecha)
        {
            Console.WriteLine("RequestOrders " + fecha.ToShortDateString());
            // Juan Castilla - Se hace una llamada directa a un stored
            /*PlannerDataSet ds = new PlannerDataSet();
            ds.EnforceConstraints = false;
            PlannerDataSetTableAdapters.PlannerOrdersTableAdapter adapter = new PlannerDataSetTableAdapters.PlannerOrdersTableAdapter();
            adapter.FillByPending(ds.PlannerOrders, fecha.ToString(),"0","0","0","0");
            //adapter.FillByPendingsp(ds.PlannerOrders, dateTime, "0", "0", "0", "0");

            var table = ds.PlannerOrders;*/
            var data = new ObservableCollection<Order>();
            using (var cmd = new SqlCommand("Orders_ByDate"))
            {
                using (var cnn = new SqlConnection(Properties.Settings.Default.EtasaConnectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime));
                    cmd.Parameters["@Date"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@Date"].Value = fecha;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newOrder = new Order
                                {
                                    Id = rdr.GetInt64(0),
                                    Reference = rdr.GetString(1),
                                    StartDate = rdr.GetDateTime(2),
                                    FinalDate = rdr.GetDateTime(3),
                                    CreatedDate = rdr.GetDateTime(4),
                                    ModifiedDate = rdr.GetDateTime(5),
                                    Location = new LocationData
                                    {
                                        Address = rdr.GetString(6),
                                        City = rdr.GetString(7),
                                        PostCode = rdr.GetString(8),
                                        Province = rdr.GetString(9),
                                        Country = rdr.GetString(10)
                                    },
                                    Factory = FactoryList.First(f=>f.Id == rdr.GetInt32(56)),
                                    Client = new Client
                                    {
                                        Id = rdr.GetInt32(38),
                                        Code = rdr.GetString(39),
                                        Name = rdr.GetString(40),
                                        Cif = rdr.GetString(41),
                                        Location = new LocationData
                                        {
                                            Address = rdr.GetString(42),
                                            City = rdr.GetString(43),
                                            PostCode = rdr.GetString(44),
                                            Province = rdr.GetString(45),
                                            Country = rdr.GetString(46),
                                            Latitude = rdr.GetFloat(53),
                                            Longitude = rdr.GetFloat(54)
                                        },
                                        Enabled = rdr.GetBoolean(55)
                                    },
                                    //{
                                    //    Id = rdr.GetInt32(67),
                                    //    Code = rdr.GetString(68),
                                    //    Name = rdr.GetString(69),
                                    //    Density = rdr.GetDecimal(70),
                                    //    MeasureUnit = rdr.GetInt16(71),
                                    //    Enabled = rdr.GetBoolean(72)
                                    //},
                                    Operator = OperatorList.First(o=>o.Id == rdr.GetInt32(22)),
                                    Observations = rdr.GetString(21),
                                    Description = rdr.GetString(20),
                                    RequestedAmount = rdr.GetInt32(14)
                                };


                                newOrder.Product = ProductList.First(p => p.Id == rdr.GetInt32(67));

                                if (!rdr.IsDBNull(74))
                                {
                                    newOrder.TripId = rdr.GetInt64(74);
                                }

                                if (!rdr.IsDBNull(81))
                                {
                                    newOrder.RouteId = rdr.GetInt64(81);
                                }

                                if (!rdr.IsDBNull(82))
                                {
                                    newOrder.AssignmentId = rdr.GetInt64(82);
                                }

                                data.Add(newOrder);
                            }
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            // Juan Castilla - Se trabaja con datos de tipo Order directamente
            //foreach (DataRow row in table.Rows)
            //{
            //    yield return ConvertToOrder(row, table);
            //}
            //foreach (var order in data)
            //{
            //    yield return order;
            //}

            Console.WriteLine("End RequestOrders " + fecha.ToShortDateString());
            return data;
        }

        public static IEnumerable<Order> RequestOrders(DateTime dateTime, string operatorsIds, string clientsIds, string factoriesIds, string productsIds)
        {
            /*Console.WriteLine("RequestOrders2 " + dateTime.ToShortDateString());
            operatorsIds = string.IsNullOrWhiteSpace(operatorsIds) ? "0" : "0," + operatorsIds;
            clientsIds = string.IsNullOrWhiteSpace(clientsIds) ? "0" : "0," + clientsIds;
            factoriesIds = string.IsNullOrWhiteSpace(factoriesIds) ? "0" : "0," + factoriesIds;
            productsIds = string.IsNullOrWhiteSpace(productsIds) ? "0" : "0," + productsIds;

            string fecha = dateTime.ToString();
            PlannerDataSet ds = new PlannerDataSet();
            ds.EnforceConstraints = false;
            PlannerDataSetTableAdapters.PlannerOrdersTableAdapter adapter = new PlannerDataSetTableAdapters.PlannerOrdersTableAdapter();
            //adapter.FillByPending(ds.PlannerOrders, dateTime.ToString(), operatorsIds, clientsIds, factoriesIds, productsIds);
            adapter.FillByPendingsp(ds.PlannerOrders, dateTime, operatorsIds, clientsIds, factoriesIds, productsIds);
            //adapter.FillByPendingtestInner(ds.PlannerOrders, dateTime.ToString() ,operatorsIds, clientsIds, factoriesIds, productsIds);



            var table = ds.PlannerOrders;
            var res = new ObservableCollection<Order>();

            foreach (DataRow row in table.Rows)
            {
                res.Add(ConvertToOrder(row, table));
            }

            foreach(var order in res)
            {
                yield return order;
            }*/

            var orders = RequestOrders(dateTime).ToList();

            if (!string.IsNullOrEmpty(operatorsIds))
            {
                var ids = operatorsIds.Split(',');
                orders = orders.Where(o => ids.Contains(o.Operator.Id.ToString()) == true).ToList();
            }

            if (!string.IsNullOrEmpty(factoriesIds))
            {
                var ids = factoriesIds.Split(',');
                orders = orders.Where(o => ids.Contains(o.Factory.Id.ToString()) == true).ToList();
            }

            if (!string.IsNullOrEmpty(clientsIds))
            {
                var ids = clientsIds.Split(',');
                orders = orders.Where(o => ids.Contains(o.Client.Id.ToString()) == true).ToList();
            }

            if (!string.IsNullOrEmpty(productsIds))
            {
                var ids = productsIds.Split(',');
                orders = orders.Where(o => ids.Contains(o.Product.Id.ToString()) == true).ToList();
            }


            foreach (var order in orders)
            {
                yield return order;
            }

            Console.WriteLine("End RequestOrders2 " + dateTime.ToShortDateString());
        }

        public static IEnumerable<Order> RequestOrders(long AssignmentId, long IdRoute)
        {
            /*PlannerDataSet ds = new PlannerDataSet
            {
                EnforceConstraints = false
            };

            PlannerDataSetTableAdapters.PlannerAssignmentsTableAdapter adapter = new PlannerDataSetTableAdapters.PlannerAssignmentsTableAdapter();
            adapter.FillByAssigments(ds.PlannerAssignments, AssignmentId, IdRoute);

            var table = ds.PlannerAssignments;
            foreach (DataRow row in table.Rows)
            {
                yield return ConvertToOrderFromPlanner(row, table);
            }*/

            var res = new ObservableCollection<Order>();
            using (var cmd = new SqlCommand("Orders_ByAssignment"))
            {
                using (var cnn = new SqlConnection(Properties.Settings.Default.EtasaConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@AssignmentId", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@RouteId", SqlDbType.BigInt));
                    cmd.Parameters["@AssignmentId"].Value = AssignmentId;
                    cmd.Parameters["@RouteId"].Value = IdRoute;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                // Juan Castilla - si no hay orden no se carga nada
                                //                 es mejor ponerlo como where en el stored
                                if (rdr.IsDBNull(0))
                                {
                                    continue;
                                }

                                var newOrder = new Order
                                {
                                    Id = rdr.GetInt64(0),
                                    AssignmentId = rdr.GetInt64(1),
                                    Reference = rdr.GetString(61),
                                    StartDate = rdr.GetDateTime(64),
                                    FinalDate = rdr.GetDateTime(65),
                                    Location = new LocationData
                                    {
                                        Address = rdr.GetString(69),
                                        City = rdr.GetString(70),
                                        PostCode = rdr.GetString(71),
                                        Province = rdr.GetString(72),
                                        Country = rdr.GetString(73)
                                    },
                                    CreatedDate = rdr.GetDateTime(67),
                                    ModifiedDate = rdr.GetDateTime(68),
                                    TripId = rdr.GetInt64(86),
                                    Status = rdr.GetInt32(84),
                                    HexColor = GetColorLoaded(rdr.GetInt64(86)),
                                    RequestedAmount = rdr.GetInt32(78)
                                };

                                if (!rdr.IsDBNull(80))
                                {
                                    newOrder.ReceivedAmount = rdr.GetInt32(80);
                                }

                                if (!rdr.IsDBNull(62))
                                {
                                    newOrder.Operator = OperatorList.First(o => o.Id == rdr.GetInt32(62));
                                }

                                if (!rdr.IsDBNull(76))
                                {
                                    newOrder.Factory = FactoryList.First(f => f.Id == rdr.GetInt32(76));
                                }

                                if (!rdr.IsDBNull(77))
                                {
                                    newOrder.Product = ProductList.First(p => p.Id == rdr.GetInt32(77));
                                }

                                if (!rdr.IsDBNull(63))
                                {
                                    newOrder.Client = new Client
                                    {
                                        Id = rdr.GetInt32(63),
                                        Code = rdr.GetString(90),
                                        Name = rdr.GetString(91),
                                        Cif = rdr.GetString(92),
                                        Enabled = rdr.GetBoolean(104),
                                        Location = new LocationData
                                        {
                                            Address = rdr.GetString(93),
                                            City = rdr.GetString(94),
                                            PostCode = rdr.GetString(95),
                                            Province = rdr.GetString(96),
                                            Country = rdr.GetString(97)
                                        },
                                        Contact = new ContactData
                                        {
                                            Phone = rdr.GetString(98),
                                            Phone2 = rdr.GetString(99),
                                            PhoneMobile = rdr.GetString(100),
                                            Email = rdr.GetString(101),
                                            Name = rdr.GetString(102),
                                            Fax = rdr.GetString(103)
                                        }
                                    };
                                }

                                res.Add(newOrder);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var x = ex.Message;
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            foreach (var order in res)
            {
                yield return order;
            }
        }

        private static Order ConvertToOrder(DataRow row, PlannerDataSet.PlannerOrdersDataTable table)
        {
            Console.WriteLine("    ConvertToOrder");
            Order order = new Order();
            string putEmergencycolor = string.Empty; 

            order.Id = Convert.ToInt64(row[table.IdColumn.ColumnName]);
            order.Reference = row[table.ReferenceColumn.ColumnName].ToString();

            order.StartDate = (DateTime)row[table.StartDateColumn.ColumnName];
            order.FinalDate = (DateTime)row[table.FinalDateColumn.ColumnName];
            order.CreatedDate = (DateTime)row[table.CreatedDateColumn.ColumnName];
            order.ModifiedDate = (DateTime)row[table.ModifiedDateColumn.ColumnName];
   

 
            order.TripId = Convert.ToInt32(row[table.TripIdColumn.ColumnName].ToString());

            LocationData location = new LocationData();
            location.Address = row[table.AddressColumn.ColumnName].ToString();
            location.City = row[table.CityColumn.ColumnName].ToString();
            location.PostCode = row[table.PostCodeColumn.ColumnName].ToString();
            location.Province = row[table.ProvinceColumn.ColumnName].ToString();
            location.Country = row[table.CountryColumn.ColumnName].ToString();


            //float.TryParse(row[table.LatitudeColumn.ColumnName].ToString(), out float loc);
            location.Latitude = (float)Convert.ToDouble(row[table.LatitudeColumn.ColumnName].ToString());

            //float.TryParse(row[table.LongitudeColumn.ColumnName].ToString(), out loc);
            location.Longitude = (float)Convert.ToDouble(row[table.LongitudeColumn.ColumnName].ToString().ToString());

            order.Location = location;

            Int32.TryParse(row[table.VehicleSizeColumn.ColumnName].ToString(), out int num);
            order.VehicleType = num;

             Int32.TryParse(row[table.RequestedAmountColumn.ColumnName].ToString(), out num);
            order.RequestedAmount = num;

            Int32.TryParse(row[table.ReceivedAmountColumn.ColumnName].ToString(), out num);
            order.ReceivedAmount = num;


            //aqui
            order.TankNum = row[table.TankNumColumn.ColumnName].ToString();
            order.TankVolume = row[table.TankVolumeColumn.ColumnName].ToString();
            order.TankLevel = row[table.TankLevelColumn.ColumnName].ToString();
            order.Status = row[table.StatusColumn.ColumnName] as int? ?? 0;
            order.Description = row[table.DescriptionColumn.ColumnName].ToString();
            order.Observations = row[table.ObservationsColumn.ColumnName].ToString();

            order.Operator = new Operator()
            {
                Id = Convert.ToInt32(row[table.OperatorIdColumn.ColumnName]),
                Code = row[table.OperatorCodeColumn.ColumnName].ToString(),
                Name = row[table.OperatorNameColumn.ColumnName].ToString(),
                Cif = row[table.OperatorCifColumn.ColumnName].ToString(),

                Location = new LocationData()
                {
                    Address = row[table.OperatorAddressColumn.ColumnName].ToString(),
                    City = row[table.OperatorCityColumn.ColumnName].ToString(),
                    PostCode = row[table.OperatorPostCodeColumn.ColumnName].ToString(),
                    Province = row[table.OperatorProvinceColumn.ColumnName].ToString(),
                    Country = row[table.OperatorCountryColumn.ColumnName].ToString(),
                },

                Contact = new ContactData()
                {
                    Name = row[table.OperatorContactColumn.ColumnName].ToString(),
                    Phone = row[table.OperatorPhoneColumn.ColumnName].ToString(),
                    Phone2 = row[table.OperatorPhone2Column.ColumnName].ToString(),
                    PhoneMobile = row[table.OperatorPhoneMobileColumn.ColumnName].ToString(),
                    Fax = row[table.OperatorFaxColumn.ColumnName].ToString(),
                    Email = row[table.OperatorEmailColumn.ColumnName].ToString()
                },


                Enabled = (bool)row[table.OperatorEnabledColumn.ColumnName],
                //Observations = row[table.OperatorObservationsColumn.ColumnName].ToString(),
            };

            order.Client = new Client()
            {
                Id = Convert.ToInt32(row[table.ClientIdColumn.ColumnName]),
                Code = row[table.ClientCodeColumn.ColumnName].ToString(),
                Name = row[table.ClientNameColumn.ColumnName].ToString(),
                Cif = row[table.ClientCifColumn.ColumnName].ToString(),

                Location = new LocationData()
                {
                    Address = row[table.ClientAddressColumn.ColumnName].ToString(),
                    City = row[table.ClientCityColumn.ColumnName].ToString(),
                    PostCode = row[table.ClientPostCodeColumn.ColumnName].ToString(),
                    Province = row[table.ClientProvinceColumn.ColumnName].ToString(),
                    Country = row[table.ClientCountryColumn.ColumnName].ToString(),

                    Latitude = (float)row[table.ClientLatitudeColumn.ColumnName],
                    Longitude = (float)row[table.ClientLongitudeColumn.ColumnName],
                },

                Contact = new ContactData()
                {
                    Name = row[table.ClientContactColumn.ColumnName].ToString(),
                    Phone = row[table.ClientPhoneColumn.ColumnName].ToString(),
                    Phone2 = row[table.ClientPhone2Column.ColumnName].ToString(),
                    PhoneMobile = row[table.ClientPhoneMobileColumn.ColumnName].ToString(),
                    Fax = row[table.ClientFaxColumn.ColumnName].ToString(),
                    Email = row[table.ClientEmailColumn.ColumnName].ToString()
                },


                Enabled = (bool)row[table.ClientEnabledColumn.ColumnName],
                //Observations = row[table.ClientObservationsColumn.ColumnName].ToString(),
            };

            order.Factory = new Factory()
            {
                Id = Convert.ToInt32(row[table.FactoryIdColumn.ColumnName]),
                Code = row[table.FactoryCodeColumn.ColumnName].ToString(),
                Name = row[table.FactoryNameColumn.ColumnName].ToString(),
                HexColor = row[table.FactoryColorColumn.ColumnName].ToString(),


                Location = new LocationData()
                {
                    Address = row[table.FactoryAddressColumn.ColumnName].ToString(),
                    City = row[table.FactoryCityColumn.ColumnName].ToString(),
                    PostCode = row[table.FactoryPostCodeColumn.ColumnName].ToString(),
                    Province = row[table.FactoryProvinceColumn.ColumnName].ToString(),
                    Country = row[table.FactoryCountryColumn.ColumnName].ToString(),
                   
                    Latitude = (float)row[table.FactoryLatitudeColumn.ColumnName],
                    Longitude = (float)row[table.FactoryLongitudeColumn.ColumnName],
                },


                Enabled = (bool)row[table.FactoryEnabledColumn.ColumnName],
                //Observations = row[table.FactoryObservationsColumn.ColumnName].ToString(),
            };

            order.Product = new Product
            {
                Id = Convert.ToInt32(row[table.ProductIdColumn.ColumnName]),
                Code = row[table.ProductCodeColumn.ColumnName].ToString(),
                Name = row[table.ProductNameColumn.ColumnName].ToString(),

                Density = Convert.ToDecimal(row[table.ProductDensityColumn.ColumnName]),
                MeasureUnit = Convert.ToInt16(row[table.ProductMeasureUnitColumn.ColumnName]),

                Enabled = Convert.ToBoolean(row[table.ProductEnabledColumn.ColumnName])
                //Observations = row[table.ProductObservationsColumn.ColumnName].ToString(),
            };

  

            order.SizeName = VehicleType.GetVehicleSizeName((int) order.VehicleType);

            Console.WriteLine("    End ConvertToOrder");
            return order;
        }

        /*
        private static Order ConvertToOrderFromPlanner(DataRow row, PlannerDataSet.PlannerAssignmentsDataTable table)
        {
            Order order = new Order();
            PlannerDataSet dataset = new PlannerDataSet();

            order.Id = Convert.ToInt64(row[table.OrderIdColumn.ColumnName]);
            Console.WriteLine("ConvertToOrderFromPlanner " + order.Id.ToString());

            order.Reference = row[table.ReferenceColumn.ColumnName].ToString();

            order.StartDate = (DateTime)row[table.StartDateColumn.ColumnName];
            order.FinalDate = (DateTime)row[table.FinalDateColumn.ColumnName];
            order.CreatedDate = (DateTime)row[table.CreatedDateColumn.ColumnName];
            order.ModifiedDate = (DateTime)row[table.ModifiedDateColumn.ColumnName];

            if (string.IsNullOrEmpty(row[table.TripIdColumn.ColumnName].ToString()))
            {
                order.TripId = 0;
            }
            else
            {
                order.TripId = Convert.ToInt64(row[table.TripIdColumn.ColumnName].ToString());
            }

            order.HexColor = GetColorLoaded(order.TripId.Value);


            LocationData location = new LocationData
            {
                Address = row[table.AddressColumn.ColumnName].ToString(),
                City = row[table.CityColumn.ColumnName].ToString(),
                PostCode = row[table.PostCodeColumn.ColumnName].ToString(),
                Province = row[table.ProvinceColumn.ColumnName].ToString(),
                Country = row[table.CountryColumn.ColumnName].ToString()
            };


            float.TryParse(row[table.LatitudeColumn.ColumnName].ToString(), out float loc);
            location.Latitude = loc;

            float.TryParse(row[table.LongitudeColumn.ColumnName].ToString(), out loc);
            location.Longitude = loc;

            order.Location = location;

            Int32.TryParse(row[table.VehicleSizeColumn.ColumnName].ToString(), out int num);
            order.VehicleType = num;

            Int32.TryParse(row[table.RequestedAmountColumn.ColumnName].ToString(), out num);
            order.RequestedAmount = num;

            Int32.TryParse(row[table.ReceivedAmountColumn.ColumnName].ToString(), out num);
            order.ReceivedAmount = num;

            //si la cantidad recivida no tiene valor se coje la que se espera requestedAmount
            if (order.ReceivedAmount == 0)
            {
                Int32.TryParse(row[table.RequestedAmountColumn.ColumnName].ToString(), out num);
                order.ReceivedAmount = num;
            }
              
            //aqui
            order.TankNum = row[table.TankNumColumn.ColumnName].ToString();
            order.TankVolume = row[table.TankVolumeColumn.ColumnName].ToString();
            order.TankLevel = row[table.TankLevelColumn.ColumnName].ToString();
            order.Status = row[table.StatusColumn.ColumnName] as int? ?? 0;
            //order.Description = row[table.DescriptionColumn.ColumnName].ToString();
            //order.Observations = row[table.ObservationsColumn.ColumnName].ToString();


            //operadores


            PlannerDataSetTableAdapters.OperatorsTableAdapter operatoradapter = new PlannerDataSetTableAdapters.OperatorsTableAdapter();
            PlannerDataSet.OperatorsDataTable dataTableOperators = operatoradapter.GetDataByOperatorId(Convert.ToInt32(row[table.OperatorIdColumn.ColumnName]));



            order.Operator = new Operator()
            {
                Id = Convert.ToInt32(row[table.OperatorIdColumn.ColumnName]),
                Code = dataTableOperators.Rows[0]["Code"].ToString(),
                Name = dataTableOperators.Rows[0]["Name"].ToString(),
                Cif = dataTableOperators.Rows[0]["Cif"].ToString(),

                Location = new LocationData()
                {
                    Address = dataTableOperators.Rows[0]["Address"].ToString(),
                    City = dataTableOperators.Rows[0]["City"].ToString(),
                    PostCode = dataTableOperators.Rows[0]["PostCode"].ToString(),
                    Province = dataTableOperators.Rows[0]["Province"].ToString(),
                    Country = dataTableOperators.Rows[0]["Country"].ToString(),       
                },

                Contact = new ContactData()
                {
                    Name = dataTableOperators.Rows[0]["Name"].ToString(),
                    Phone = dataTableOperators.Rows[0]["Phone"].ToString(),
                    Phone2 = dataTableOperators.Rows[0]["Phone2"].ToString(),
                    PhoneMobile = dataTableOperators.Rows[0]["PhoneMobile"].ToString(),
                    Fax = dataTableOperators.Rows[0]["Fax"].ToString(),
                    Email = dataTableOperators.Rows[0]["Email"].ToString()
                },


                Enabled = (bool)dataTableOperators.Rows[0]["Enabled"],
                //Observations = row[table.ClientObservationsColumn.ColumnName].ToString(),
            };


            //clientes 

            PlannerDataSetTableAdapters.ClientsTableAdapter adapter = new PlannerDataSetTableAdapters.ClientsTableAdapter();
            PlannerDataSet.ClientsDataTable dataTableClients = adapter.GetDataByIdClient(Convert.ToInt32(row[table.ClientIdColumn.ColumnName]));
          
            order.Client = new Client()
            {
                Id = Convert.ToInt32(dataTableClients.Rows[0]["Id"].ToString()),
                Code = dataTableClients.Rows[0]["Code"].ToString(),
                Name = dataTableClients.Rows[0]["Name"].ToString(),
                Cif = dataTableClients.Rows[0]["Cif"].ToString(),

                Location = new LocationData()
                {
                    Address = dataTableClients.Rows[0]["Address"].ToString(),
                    City = dataTableClients.Rows[0]["City"].ToString(),
                    PostCode = dataTableClients.Rows[0]["PostCode"].ToString(),
                    Province = dataTableClients.Rows[0]["Province"].ToString(),
                    Country = dataTableClients.Rows[0]["Country"].ToString(),

                    Latitude = (float)dataTableClients.Rows[0]["Latitude"],
                    Longitude = (float)dataTableClients.Rows[0]["Longitude"],
                },

                Contact = new ContactData()
                {
                    Name = dataTableClients.Rows[0]["Name"].ToString(),
                    Phone = dataTableClients.Rows[0]["Phone"].ToString(),
                    Phone2 = dataTableClients.Rows[0]["Phone2"].ToString(),
                    PhoneMobile = dataTableClients.Rows[0]["PhoneMobile"].ToString(),
                    Fax = dataTableClients.Rows[0]["Fax"].ToString(),
                    Email = dataTableClients.Rows[0]["Email"].ToString()
                },


                Enabled = (bool)dataTableClients.Rows[0]["Enabled"],
                //Observations = row[table.ClientObservationsColumn.ColumnName].ToString(),
            };

  
            //factorias 

            PlannerDataSetTableAdapters.FactoriesTableAdapter factoryadapter = new PlannerDataSetTableAdapters.FactoriesTableAdapter();
            PlannerDataSet.FactoriesDataTable factory = factoryadapter.GetDataByIdFactory(Convert.ToInt32(row[table.FactoryIdColumn.ColumnName]));


            order.Factory = new Factory()
            {
                Id = Convert.ToInt32(factory.Rows[0]["Id"].ToString()),
                Code = factory.Rows[0]["Code"].ToString(),
                Name = factory.Rows[0]["Name"].ToString(),

                HexColor = GetColorFactory(Convert.ToInt32(row[table.FactoryIdColumn.ColumnName].ToString())),

                Location = new LocationData()
                {
                    Address = factory.Rows[0]["Address"].ToString(),
                    City = factory.Rows[0]["City"].ToString(),
                    PostCode = factory.Rows[0]["PostCode"].ToString(),
                    Province = factory.Rows[0]["Province"].ToString(),
                    Country = factory.Rows[0]["Country"].ToString(),

                    Latitude = (float)factory.Rows[0]["Latitude"],
                    Longitude = (float)factory.Rows[0]["Longitude"],
                },


                Enabled = (bool)factory.Rows[0]["Enabled"],
                //Observations = row[table.FactoryObservationsColumn.ColumnName].ToString(),
            };

            //producto
            PlannerDataSetTableAdapters.ProductsTableAdapter productsadapter = new PlannerDataSetTableAdapters.ProductsTableAdapter();
            PlannerDataSet.ProductsDataTable product = productsadapter.GetDataProductId(Convert.ToInt32(row[table.ProductIdColumn.ColumnName]));

            order.Product = new Product()
            {
                Id = Convert.ToInt32(product.Rows[0]["Id"].ToString()),
                Code = product.Rows[0]["Code"].ToString(),
                Name = product.Rows[0]["Name"].ToString(),

                Density = 4,
               MeasureUnit = 4,
            };
            

            order.SizeName = VehicleType.GetVehicleSizeName((int)order.VehicleType);

            //actualizamos el estado del pedido a planificado  
            //UpdateStatusOrders(order.Id);


            Console.WriteLine("End ConvertToOrderFromPlanner");
            return order;
        }
        */
        public static string GetColorFactory(int idFactory)
        {
            string color = Colors.Gray.ToString();
            PlannerDataSet ds = new PlannerDataSet();
            ds.EnforceConstraints = false;
            DataTable Colour = new DataTable();
            PlannerDataSetTableAdapters.FactoriesColorsTableAdapter adapter = new PlannerDataSetTableAdapters.FactoriesColorsTableAdapter();
            adapter.FillBy(ds.FactoriesColors, idFactory);

           var table = ds.FactoriesColors;

           if (table.Rows.Count > 0)
           {
                color = table.Rows[0]["FactoryColor"].ToString().Trim();
           }
           return color;
        }

        public static string GetColorLoaded(long orderTripId)
        {
            PlannerDataSet ds = new PlannerDataSet();
            ds.EnforceConstraints = false;
            string color = "#FE2E2E";
            DataTable tripsdatatable = new DataTable();
            int status = 0;
            PlannerDataSetTableAdapters.TripsTableAdapter adapter = new PlannerDataSetTableAdapters.TripsTableAdapter();
            tripsdatatable = adapter.GetDataByIdTrips(orderTripId);
    
            if (tripsdatatable.Rows.Count > 0)
            {
                status = Convert.ToInt32(tripsdatatable.Rows[0]["Status"].ToString().Trim());
                if (status == 3)
                {
                    DataTable Colour = new DataTable();
                    PlannerDataSetTableAdapters.StatusTableAdapter adapter2 = new PlannerDataSetTableAdapters.StatusTableAdapter();
                    adapter2.FillByStatus(ds.Status, status);
                    var table = ds.Status;
                    if (table.Rows.Count > 0)
                    {
                        color = table.Rows[0]["Color"].ToString().Trim();
                    }
                }         
            }    
            return color;
        }





        /*
        //ponemos el status del pedido a planificad 
        public static void UpdateStatusOrders(long idOrder)
        {
            var con = Properties.Settings.Default.EtasaConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE Orders SET Status = @Status WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", idOrder);
                    cmd.Parameters.AddWithValue("@Status", 2);
                    cmd.ExecuteScalar();
                }

            }
            catch(Exception ex)
            {

            }
     
        }
        */

        public static IEnumerable<Assignment> RequestAssignments(DateTime dateTime, string filterVehicle)
        {
            /*//si no hay filtro por vehiculo no realizamos ninguna acción   
            if (string.IsNullOrEmpty(filterVehicle))
            {*/
            var res = new List<Assignment>();
            using (var cmd = new SqlCommand(string.IsNullOrEmpty(filterVehicle) ? "Assignments_ByDate" : "Assignments_ByDateAndVehicle"))
            {
                using (var cnn = new SqlConnection(Properties.Settings.Default.EtasaConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime));
                    cmd.Parameters["@Date"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@Date"].Value = dateTime;

                    if (!string.IsNullOrEmpty(filterVehicle))
                    {
                        cmd.Parameters.Add(new SqlParameter("@TrailersId", SqlDbType.NVarChar));
                        cmd.Parameters["@TrailersId"].Direction = ParameterDirection.Input;
                        cmd.Parameters["@TrailersId"].Value = filterVehicle;
                    }

                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                /*var newAssignment = new Assignment
                                {
                                    Id = rdr.GetInt64(0),
                                    Date = rdr.GetDateTime(1),
                                    CreatedTime = rdr.GetDateTime(2),
                                    ModifiedDate = rdr.GetDateTime(3),
                                    Repeat = rdr.GetBoolean(4),
                                    Enabled = rdr.GetBoolean(5),
                                    FactoryId = rdr.GetInt32(21),
                                    FactoryName = rdr.GetString(22),
                                    MessageOverAmount = rdr.GetString(25),
                                    Observations = rdr.GetString(23),
                                    TripId = rdr.GetInt64(24)
                                };

                                newAssignment.Driver = new Driver
                                {
                                    Id = rdr.GetInt32(6),
                                    Code = rdr.GetString(7),
                                    Name = rdr.GetString(8),
                                };

                                newAssignment.Cab = new Cab
                                {
                                    Id = rdr.GetInt32(9),
                                    Code = rdr.GetString(10),
                                    LicensePlate = rdr.GetString(11),
                                    VIN = rdr.GetString(12),
                                    MaxWeight = rdr.GetInt32(13)
                                };

                                newAssignment.Trailer = new Trailer
                                {
                                    Id = rdr.GetInt32(14),
                                    Code = rdr.GetString(15),
                                    LicensePlate = rdr.GetString(16),
                                    VIN = rdr.GetString(17),
                                    MaxWeight = rdr.GetInt32(18),
                                    TankVolume = rdr.GetInt32(19)
                                };

                                if (!rdr.IsDBNull(20))
                                {
                                    newAssignment.RoutesId = rdr.GetInt64(20);
                                }*/

                                var newAssignment = new Assignment
                                {
                                    Id = rdr.GetInt64(1),
                                    Date = rdr.GetDateTime(2),
                                    CreatedTime = rdr.GetDateTime(3),
                                    ModifiedDate = rdr.GetDateTime(4),
                                    Repeat = rdr.GetBoolean(5),
                                    Enabled = rdr.GetBoolean(6),
                                    Driver = new Driver
                                    {
                                        Id = rdr.GetInt32(7),
                                        Code = rdr.GetString(8),
                                        Name = rdr.GetString(9)
                                    },
                                    Cab = new Cab
                                    {
                                        Id = rdr.GetInt32(10),
                                        Code = rdr.GetString(11),
                                        LicensePlate = rdr.GetString(12),
                                        VIN = rdr.GetString(13)
                                    },
                                    Trailer = new Trailer
                                    {
                                        Id = rdr.GetInt32(14),
                                        Code = rdr.GetString(15),
                                        LicensePlate = rdr.GetString(16),
                                        VIN = rdr.GetString(17),
                                        MaxWeight = rdr.GetInt32(18),
                                        TankVolume = rdr.GetInt32(19)
                                    },
                                    TotalAmountAssigment = rdr.GetInt32(20),
                                    MessageOverAmount = rdr.GetString(21),
                                    TripId = 0,
                                    FactoryId = 0,
                                    FactoryName = string.Empty
                                };

                                if (!rdr.IsDBNull(22))
                                {
                                    newAssignment.TripId = rdr.GetInt64(22);
                                }

                                if (!rdr.IsDBNull(23))
                                {
                                    newAssignment.RoutesId = rdr.GetInt64(23);
                                }

                                if (!rdr.IsDBNull(24))
                                {
                                    newAssignment.FactoryId = rdr.GetInt32(24);
                                    newAssignment.FactoryName = FactoryList.First(f => f.Id == rdr.GetInt32(24)).Name;
                                }

                                GetLastVehicleLocation(newAssignment.Cab.Id);

                                res.Add(newAssignment);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var x = "";
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }

                    foreach (var assignment in res)
                    {
                        yield return assignment;
                    }
                }
            }
            /*}
            //filtramos la misma consulta
            else
            {
                PlannerDataSet ds = new PlannerDataSet();
                ds.EnforceConstraints = false;
                string WhereQueryFilterVehicle = "";
                PlannerDataSetTableAdapters.PlannerAssignmentsTableAdapter adapt = new PlannerDataSetTableAdapters.PlannerAssignmentsTableAdapter();

                WhereQueryFilterVehicle = string.IsNullOrWhiteSpace(filterVehicle) ? "0" : "0," + filterVehicle;
                adapt.FillByDateAndVehicleIds(ds.PlannerAssignments, dateTime.ToString(), WhereQueryFilterVehicle); var table = ds.PlannerAssignments;

                foreach (DataRow row in table.Rows)
                {
                    yield return ConvertToAssignment(row, table);
                }
            }*/
        }

        private static Assignment ConvertToAssignment(DataRow row, PlannerDataSet.PlannerAssignmentsDataTable table)
        {
            Assignment assignment = new Assignment();
            assignment.Id = Convert.ToInt32(row[table.AssigmentIdColumn.ColumnName]);
            var driverId = Convert.ToInt32(row[table.DriverIdColumn.ColumnName]);
            var cabId = Convert.ToInt32(row[table.CabIdColumn.ColumnName]);
            var trailerId = Convert.ToInt32(row[table.TrailerIdColumn.ColumnName]);
            var totalamount = Convert.ToInt32(row[table.TotalAmountColumn.ColumnName]);
            assignment.MessageOverAmount = row[table.MessageOverAmountColumn.ColumnName].ToString();

            if (string.IsNullOrEmpty(row[table.FactoryIdColumn.ColumnName].ToString()))
            {
                assignment.FactoryId = 0;
            }
            else
            {
                assignment.FactoryId = Convert.ToInt32(row[table.FactoryIdColumn.ColumnName].ToString());
            }      
            assignment.TotalAmountAssigment = totalamount;

           var isRigid = cabId == trailerId;
    
           if ( string.IsNullOrEmpty(row[table.TripIdColumn.ColumnName].ToString()))
            {
                assignment.TripId = 0;
                assignment.FactoryName = string.Empty;
            }

           else
            {
                assignment.TripId = Convert.ToInt32(row[table.TripIdColumn.ColumnName]);
                
                assignment.FactoryName = GetFactoryName((int)assignment.FactoryId);
            }
                 
            assignment.RoutesId = Convert.ToInt32(row[table.routesId6Column.ColumnName]);

            if (driverId > 0 && cabId > 0 && trailerId > 0)
            {
                Driver driver = new Driver()
                {
                    Id = driverId,
                    Code = row[table.DriverCodeColumn.ColumnName].ToString(),
                    Name = row[table.DriverNameColumn.ColumnName].ToString(),
                };
                assignment.Driver = driver;


                Cab cab = new Cab()
                {
                    Id = cabId,
                    Code = row[table.CabCodeColumn.ColumnName].ToString(),
                    LicensePlate = row[table.CabLicensePlateColumn.ColumnName].ToString(),
                    VIN = row[table.CabVINColumn.ColumnName].ToString(),
                    MaxWeight = Convert.ToInt32(row[table.CabMaxWeightColumn.ColumnName])
                };
                cab.Location = GetLastVehicleLocation(cab.Id);
                assignment.Cab = cab;


                Trailer trailer = new Trailer()
                {
                    Id = trailerId,
                    Code = row[table.TrailerCodeColumn.ColumnName].ToString(),
                    LicensePlate = row[table.TrailerLicensePlateColumn.ColumnName].ToString(),
                    VIN = row[table.TrailerVINColumn.ColumnName].ToString(),
                    MaxWeight = Convert.ToInt32(row[table.TrailerMaxWeightColumn.ColumnName]),
                    TankVolume = Convert.ToInt32(row[table.TrailerTankVolumeColumn.ColumnName])
                };
                trailer.Location = isRigid ? GetLastVehicleLocation(cab.Id) : cab.Location;
                assignment.Trailer = trailer;


                assignment.Date = (DateTime)row[table.DateColumn.ColumnName];
                assignment.CreatedTime = (DateTime)row[table.CreatedDateColumn.ColumnName];
                assignment.ModifiedDate = (DateTime)row[table.ModifiedDateColumn.ColumnName];
                assignment.Repeat = (bool)row[table.RepeatColumn.ColumnName];
                assignment.Enabled = (bool)row[table.EnabledColumn.ColumnName];
                //assignment.Observations = row[table.ObservationsColumn.ColumnName].ToString();
                return assignment;
            }
            return null;
        }

        private static string GetFactoryName(int tripId)
        {
            string sFactoryName = string.Empty;
            PlannerDataSet ds = new PlannerDataSet();
            PlannerDataSetTableAdapters.FactoriesTableAdapter adapt = new PlannerDataSetTableAdapters.FactoriesTableAdapter();
            adapt.FillByTrip(ds.Factories, tripId);

            if (ds.Factories.Count > 0)
                sFactoryName = ds.Factories[0].Name;

            return sFactoryName;
        }

        private static VehicleLocationData GetLastVehicleLocation(int vehicleId)
        {
            /*PlannerDataSet ds = new PlannerDataSet();
            ds.EnforceConstraints = false;
            PlannerDataSetTableAdapters.Vehicles_LocationsTableAdapter adapt = new PlannerDataSetTableAdapters.Vehicles_LocationsTableAdapter();
            adapt.FillLastBy(ds.Vehicles_Locations, vehicleId);

            if (ds.Vehicles_Locations.Rows.Count > 0)
            {
                var row = ds.Vehicles_Locations.Rows[0];

                return new VehicleLocationData
                {
                    Latitude = Convert.ToDouble(row[ds.Vehicles_Locations.LatitudeColumn.ColumnName]),
                    Longitude = Convert.ToDouble(row[ds.Vehicles_Locations.LongitudeColumn.ColumnName]),
                    TimeStamp = (DateTime)row[ds.Vehicles_Locations.TimeStampColumn.ColumnName],
                };
            }
            return null;*/

            using (var cmd = new SqlCommand("SELECT TOP 1 Id, VehicleId, Latitude, Longitude, TimeStamp FROM Vehicles_Locations WITH(NOLOCK) WHERE VehicleId = " + vehicleId.ToString() + " ORDER BY TimeStamp DESC"))
            {
                using (var cnn = new SqlConnection(Properties.Settings.Default.EtasaConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            if (rdr.HasRows)
                            {
                                return new VehicleLocationData
                                {
                                    Latitude = Convert.ToDouble(rdr.GetFloat(2)),
                                    Longitude = Convert.ToDouble(rdr.GetFloat(3)),
                                    TimeStamp = rdr.GetDateTime(4)
                                };
                            }
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return null;
        }

        public static IEnumerable<FactoryColors> RequestColors()
        {
            PlannerDataSet ds = new PlannerDataSet();
            ds.EnforceConstraints = false;
            PlannerDataSetTableAdapters.FactoriesColorsTableAdapter adapt = new PlannerDataSetTableAdapters.FactoriesColorsTableAdapter();
            adapt.Fill(ds.FactoriesColors);

            var table = ds.FactoriesColors;

            foreach (DataRow row in table.Rows)
            {
                FactoryColors color = new FactoryColors
                {
                    FactoryId = Convert.ToInt32(row[table.IdColumn.ColumnName]),
                    FactoryColor = GetColor(row[table.FactoryColorColumn.ColumnName].ToString()),
                    ClientColor = GetColor(row[table.ClientColorColumn.ColumnName].ToString()),
                    UrgentColor = GetColor(row[table.UrgentColorColumn.ColumnName].ToString()),
                    FinalDayColor = GetColor(row[table.FinalDayColorColumn.ColumnName].ToString()),
                    PreferenceColor = GetColor(row[table.PreferenceColorColumn.ColumnName].ToString())
                };

                yield return color;
            }
        }
        private static Color GetColor(String color)
        {
            try
            {
                return ColorConverter.ConvertFromString(color) as Color? ?? Colors.Gray;
            }
            catch
            {
                return Colors.Gray;
            }
        }


        public static void AddPlannedOrder(long tripId, Order order)
        {
            var con = Properties.Settings.Default.EtasaConnectionString;
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE Orders SET TripId = @TripId, Status = @Status WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@TripId", tripId);
                cmd.Parameters.AddWithValue("@Id", order.Id);
                cmd.Parameters.AddWithValue("@Status", 2);
                cmd.ExecuteScalar();

                order.Status = 2;
            }
        }
        
        public static void RemovePlannedOrder(Order order)
        {
            var con = Properties.Settings.Default.EtasaConnectionString;
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE Orders SET AssignmentId = @NULL, AssignmentPosition = @NULL, Status = @Status WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@NULL", DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", order.Id);
                cmd.Parameters.AddWithValue("@Status", 1);
                cmd.ExecuteScalar();

                order.Status = 1;
            }
        }
        
        public static void UpdateAssignmentOrderPosition(Order order, int position)
        {
            var con = Properties.Settings.Default.EtasaConnectionString;
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE Orders SET AssignmentPosition = @AssignmentPosition WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@AssignmentPosition", position);
                cmd.Parameters.AddWithValue("@Id", order.Id);
                cmd.ExecuteScalar();
            }
        }

        public static void UpdateOrder(Order order)
        {
            PlannerDataSet ds = new PlannerDataSet();
            PlannerDataSetTableAdapters.OrdersTableAdapter tableAdapter = new PlannerDataSetTableAdapters.OrdersTableAdapter();
            tableAdapter.FillById(ds.Orders, (int) order.Id);

            if(ds.Orders.Count > 0)
            {
                var orderDb = ds.Orders[0];

                orderDb.StartDate = order.StartDate;
                orderDb.FinalDate = order.FinalDate;
                orderDb.RequestedAmount = order.RequestedAmount;

                orderDb.Status = order.Status;


                tableAdapter.Update(ds.Orders);
            }
            
        }
    }
}
