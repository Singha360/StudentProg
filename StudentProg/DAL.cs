/*
* Ceci est un fichier de la couche DAL (Data Access Layer)
* Il contient les classes qui permettent de se connecter à la base de données
* et de récupérer les données de celle-ci.
* 
*/

using System;
using System.Data.SqlClient;
using System.Data;

namespace StudentProgData
{
    internal class Connect
    {
        private static String connectionString = getConnectionString(); // Fait appel à la mthode getConnectionString() pour récupérer la connection string
        internal static String ConnectionString
        {
            get { return connectionString; } // Getter de la connection string
        }
        private static String getConnectionString()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.DataSource = "127.0.0.1"; //Changez cela a soit 127.0.0.1 ou au nom de l'utilisateur de l'ordi
            connectionStringBuilder.InitialCatalog = "FORMATIVE"; // change celui-ci pour le nom de ta base de données (database)
            connectionStringBuilder.UserID = "sa";
            connectionStringBuilder.Password = "sysadm";
            Console.WriteLine(connectionStringBuilder.ConnectionString);
            return connectionStringBuilder.ConnectionString;
        }
    }

    internal class DataTables
    {
        private static SqlDataAdapter adapterStudents = InitAdapterStudents();
        private static SqlDataAdapter adapterPrograms = InitAdapterPrograms();

        private static DataSet ds = InitDataSet();

        private static SqlDataAdapter InitAdapterStudents()
        {
            SqlDataAdapter studentAdapter = new SqlDataAdapter(
                "SELECT * FROM Students ORDER BY StudID",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(studentAdapter);
            // Au cas où il y ait des conflits, on écrase les changements
            builder.ConflictOption = ConflictOption.OverwriteChanges;
            studentAdapter.UpdateCommand = builder.GetUpdateCommand();

            return studentAdapter;
        }

        private static SqlDataAdapter InitAdapterPrograms()
        {
            SqlDataAdapter programAdapter = new SqlDataAdapter(
                "SELECT * FROM Programs ORDER BY ProgID",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(programAdapter);
            programAdapter.UpdateCommand = builder.GetUpdateCommand();

            return programAdapter;
        }

        private static DataSet InitDataSet()
        {
            DataSet ds = new DataSet();
            loadPrograms(ds);
            loadStudents(ds);
            return ds;
        }

        private static void loadStudents(DataSet ds)
        {
            adapterStudents.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            adapterStudents.Fill(ds, "Students");

            ForeignKeyConstraint myFK = new ForeignKeyConstraint("MyFK",
                new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgID"]
                },
                new DataColumn[] {
                    ds.Tables["Students"].Columns["ProgID"],
                }
            );
            myFK.DeleteRule = Rule.None;
            myFK.UpdateRule = Rule.Cascade;
            ds.Tables["Students"].Constraints.Add(myFK);

        }

        private static void loadPrograms(DataSet ds)
        {
            adapterStudents.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            adapterPrograms.Fill(ds, "Programs");


        }

        internal static SqlDataAdapter getAdapterPrograms()
        {
            return adapterPrograms;
        }
        internal static SqlDataAdapter getAdapterStudents()
        {
            return adapterStudents;
        }
        internal static DataSet getDataSet()
        {
            return ds;
        }
    }

    internal class Programs
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterPrograms();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetPrograms()
        {
            return ds.Tables["Programs"];
        }

        internal static int UpdatePrograms()
        {
            if (!ds.Tables["Programs"].HasErrors)
            {
                return adapter.Update(ds.Tables["Programs"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Students
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterStudents();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetStudents()
        {
            return ds.Tables["Students"];
        }

        internal static int UpdateStudents()
        {
            if (!ds.Tables["Students"].HasErrors)
            {
                return adapter.Update(ds.Tables["Students"]);
            }
            else
            {
                return -1;
            }
        }
    }
}

