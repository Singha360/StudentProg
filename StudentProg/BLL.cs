using System.Data;

namespace BusinessLayer
{
    internal class Programs
    {
        internal static int UpdatePrograms()
        {
            // =========================================================================
            //  Business rules for Programs
            // =========================================================================

            return StudentProgData.Programs.UpdatePrograms();
        }
    }

    internal class Students
    {
        internal static int UpdateStudents()
        {
            // =========================================================================
            //  Business rules for Students
            // =========================================================================

            DataTable dt = StudentProgData.Students.GetStudents()
                              .GetChanges(DataRowState.Added | DataRowState.Modified);

            if ((dt != null) && ((dt.Select("YearEnrolment < 2017").Length > 0) || (dt.Select("YearEnrolment > 2023").Length > 0))) // Une facon plus lisible sera en dessous
            {
                StudentProg.Form1.badYear();
                StudentProgData.Students.GetStudents().RejectChanges();
                return -1;
            }
            else
            {
                return StudentProgData.Students.UpdateStudents();
            }

            /*
            * Une facon plus lisible pour ligne 28+
            *
            */

            //if (dt != null)
            //{
            //    if (dt.Select("YearEnrolment < 2017").Length > 0 || dt.Select("YearEnrolment > 2023").Length > 0))
            //    {
            //        StudentProg.Form1.badYear();
            //        StudentProgData.Students.GetStudents().RejectChanges();
            //        return -1;
            //    }
            //    else
            //    {
            //        return StudentProgData.Students.UpdateStudents();
            //    }
            //}
        }
    }
}
