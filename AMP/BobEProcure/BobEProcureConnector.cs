using AMP.BobEProcure.Models;
using AMP.Models;
using AMP.Services;
using Npgsql;
using System;
using System.Configuration;
using System.Data;
using System.Linq;

namespace AMP.BobEProcure
{
    public class BobEProcureConnector
    {
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["BobEProcureEntites"].ToString();
            }
        }
        private int? ToNullableInt(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }
        private DateTime? ToNullableDateTime(string s)
        {
            DateTime d;
            if (DateTime.TryParse(s, out d)) return d;
            return null;
        }
        private decimal? ToNullableDecimal(string s)
        {
            decimal d;
            if (decimal.TryParse(s, out d)) return d;
            return null;
        }

        public bool PackageSync(out string exception)
        {
            exception = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                conn.Open();
                string sql = "SELECT pq_id, pq_ref_no, pq_title, pq_type, pq_country, pq_approver, status, created_by, created_date, publish_pq_date, pq_drafted_date, project_info, loc_number, primary_jv_percentage, secondary_jv_percentage, loc_amount, type_of_pq, serial_no, pq_no, no_of_package, package_duration, lead_minimum_share, jv_minimum_share, package_id, package_name, package_value, index, package_disp_id, estimated_value, average_requirement, cash_flow_requirement FROM \"lmp-dev\".v_pq_packages; ";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                da.Fill(ds);
                dt = ds.Tables[0];
                conn.Close();
            }
            catch (Exception msg)
            {
                new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                {
                    FullMessage = msg.ToString(),
                    Message = string.Format("Error occured while fetching data. Error: {0}",msg.Message),
                    ServiceName = "PackageSync.BobEProcureConnector",
                });
                exception += msg.Message;
            }
            if (ds.Tables.Count > 0)
            {
                #region Insert Or Update Records
                using (AMPEntities context = new AMPEntities())
                {
                    try
                    {
                        #region Update
                        var ToUpdate = (
                                    from pac in dt.Select().AsEnumerable()
                                    join p in context.BobEProcure_Packages on pac["package_id"].ToString() equals p.PackageId
                                    where (p.PQId != pac["pq_id"].ToString()) || (p.Ref != pac["pq_ref_no"].ToString()) || (p.Title != pac["pq_title"].ToString()) || (p.Type != pac["pq_type"].ToString()) ||
                                          (p.Country != pac["pq_country"].ToString()) || (p.Approver != pac["pq_approver"].ToString()) || (p.Status != ToNullableInt(pac["status"].ToString())) ||
                                          (p.CreatedBy != pac["created_by"].ToString()) || (p.CreatedOn != ToNullableDateTime(pac["created_date"].ToString())) ||
                                          (p.PublishDate != ToNullableDateTime(pac["publish_pq_date"].ToString())) || (p.DraftDate != ToNullableDateTime(pac["pq_drafted_date"].ToString())) ||
                                          (p.ProjectInfo != pac["project_info"].ToString()) || (p.LocNumber != pac["loc_number"].ToString()) || (p.PrimaryJvPercentage != ToNullableDecimal(pac["primary_jv_percentage"].ToString())) ||
                                          (p.SecondaryJvPercentage != ToNullableDecimal(pac["secondary_jv_percentage"].ToString())) || (p.LocAmount != ToNullableDecimal(pac["loc_amount"].ToString())) ||
                                          (p.TypeOfPackage != pac["type_of_pq"].ToString()) || (p.SerialNo != ToNullableInt(pac["serial_no"].ToString())) || (p.PQNo != pac["pq_no"].ToString()) ||
                                          (p.NoOfPackage != ToNullableInt(pac["no_of_package"].ToString())) || (p.PackageDuration != ToNullableDecimal(pac["package_duration"].ToString())) ||
                                          (p.LeadMinimumShare != ToNullableInt(pac["lead_minimum_share"].ToString())) || (p.JvMinShare != ToNullableInt(pac["jv_minimum_share"].ToString())) ||
                                          (p.PackageName != pac["package_name"].ToString()) || (p.PackageValue != ToNullableDecimal(pac["package_value"].ToString())) || (p.Index != ToNullableInt(pac["index"].ToString())) ||
                                          (p.PackageDisplayId != pac["package_disp_id"].ToString()) || (p.EstimateValue != ToNullableDecimal(pac["estimated_value"].ToString())) ||
                                          (p.AverageRequirement != ToNullableDecimal(pac["average_requirement"].ToString())) || (p.CashFlowRequirement != ToNullableDecimal(pac["cash_flow_requirement"].ToString()))
                                    select new Packages()
                                    {
                                        PQId = pac["pq_id"].ToString(),
                                        Ref = pac["pq_ref_no"].ToString(),
                                        Title = pac["pq_title"].ToString(),
                                        Type = pac["pq_type"].ToString(),
                                        Country = pac["pq_country"].ToString(),
                                        Approver = pac["pq_approver"].ToString(),
                                        Status = ToNullableInt(pac["status"].ToString()),
                                        CreatedBy = pac["created_by"].ToString(),
                                        CreatedOn = ToNullableDateTime(pac["created_date"].ToString()),
                                        PublishDate = ToNullableDateTime(pac["publish_pq_date"].ToString()),
                                        DraftDate = ToNullableDateTime(pac["pq_drafted_date"].ToString()),
                                        ProjectInfo = pac["project_info"].ToString(),
                                        LocNumber = pac["loc_number"].ToString(),
                                        PrimaryJvPercentage = ToNullableDecimal(pac["primary_jv_percentage"].ToString()),
                                        SecondaryJvPercentage = ToNullableDecimal(pac["secondary_jv_percentage"].ToString()),
                                        LocAmount = ToNullableDecimal(pac["loc_amount"].ToString()),
                                        TypeOfPackage = pac["type_of_pq"].ToString(),
                                        SerialNo = ToNullableInt(pac["serial_no"].ToString()),
                                        PQNo = pac["pq_no"].ToString(),
                                        NoOfPackage = ToNullableInt(pac["no_of_package"].ToString()),
                                        PackageDuration = ToNullableDecimal(pac["package_duration"].ToString()),
                                        LeadMinimumShare = ToNullableInt(pac["lead_minimum_share"].ToString()),
                                        JvMinShare = ToNullableInt(pac["jv_minimum_share"].ToString()),
                                        PackageName = pac["package_name"].ToString(),
                                        PackageValue = ToNullableDecimal(pac["package_value"].ToString()),
                                        Index = ToNullableInt(pac["index"].ToString()),
                                        PackageDisplayId = pac["package_disp_id"].ToString(),
                                        EstimateValue = ToNullableDecimal(pac["estimated_value"].ToString()),
                                        AverageRequirement = ToNullableDecimal(pac["average_requirement"].ToString()),
                                        CashFlowRequirement = ToNullableDecimal(pac["cash_flow_requirement"].ToString()),
                                        Id = p.Id
                                    }
                                ).ToList();
                        foreach (var record in ToUpdate)
                        {
                            var data = context.BobEProcure_Packages.FirstOrDefault(e => e.Id == record.Id);
                            if (data != null)
                            {
                                data.PQId = record.PQId;
                                data.Ref = record.Ref;
                                data.Title = record.Title;
                                data.Type = record.Type;
                                data.Country = record.Country;
                                data.Approver = record.Approver;
                                data.Status = record.Status;
                                data.CreatedBy = record.CreatedBy;
                                data.CreatedOn = record.CreatedOn;
                                data.PublishDate = record.PublishDate;
                                data.DraftDate = record.DraftDate;
                                data.ProjectInfo = record.ProjectInfo;
                                data.LocNumber = record.LocNumber;
                                data.PrimaryJvPercentage = record.PrimaryJvPercentage;
                                data.SecondaryJvPercentage = record.SecondaryJvPercentage;
                                data.LocAmount = record.LocAmount;
                                data.TypeOfPackage = record.TypeOfPackage;
                                data.SerialNo = record.SerialNo;
                                data.PQNo = record.PQNo;
                                data.NoOfPackage = record.NoOfPackage;
                                data.PackageDuration = record.PackageDuration;
                                data.LeadMinimumShare = record.LeadMinimumShare;
                                data.JvMinShare = record.JvMinShare;
                                data.PackageName = record.PackageName;
                                data.PackageValue = record.PackageValue;
                                data.Index = record.Index;
                                data.PackageDisplayId = record.PackageDisplayId;
                                data.EstimateValue = record.EstimateValue;
                                data.AverageRequirement = record.AverageRequirement;
                                data.CashFlowRequirement = record.CashFlowRequirement;
                            }
                        }
                        #endregion
                    }
                    catch(Exception ex)
                    {
                        new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                        {
                            FullMessage = ex.ToString(),
                            Message = string.Format("Error occured while parsing for update. Error: {0}", ex.Message),
                            ServiceName = "PackageSync.BobEProcureConnector",
                        });
                        exception += ex.Message;
                    }
                    try
                    {
                        #region Insert
                        var ToInsert = (
                                    from pac in dt.Select().AsEnumerable()
                                    join p in context.BobEProcure_Packages on pac["package_id"].ToString() equals p.PackageId into recs
                                    from rt in recs.DefaultIfEmpty()
                                    where rt == null
                                    select new BobEProcure_Packages()
                                    {
                                        PQId = pac["pq_id"].ToString(),
                                        PackageId = pac["package_id"].ToString(),
                                        Ref = pac["pq_ref_no"].ToString(),
                                        Title = pac["pq_title"].ToString(),
                                        Type = pac["pq_type"].ToString(),
                                        Country = pac["pq_country"].ToString(),
                                        Approver = pac["pq_approver"].ToString(),
                                        Status = ToNullableInt(pac["status"].ToString()),
                                        CreatedBy = pac["created_by"].ToString(),
                                        CreatedOn = ToNullableDateTime(pac["created_date"].ToString()),
                                        PublishDate = ToNullableDateTime(pac["publish_pq_date"].ToString()),
                                        DraftDate = ToNullableDateTime(pac["pq_drafted_date"].ToString()),
                                        ProjectInfo = pac["project_info"].ToString(),
                                        LocNumber = pac["loc_number"].ToString(),
                                        PrimaryJvPercentage = ToNullableDecimal(pac["primary_jv_percentage"].ToString()),
                                        SecondaryJvPercentage = ToNullableDecimal(pac["secondary_jv_percentage"].ToString()),
                                        LocAmount = ToNullableDecimal(pac["loc_amount"].ToString()),
                                        TypeOfPackage = pac["type_of_pq"].ToString(),
                                        SerialNo = ToNullableInt(pac["serial_no"].ToString()),
                                        PQNo = pac["pq_no"].ToString(),
                                        NoOfPackage = ToNullableInt(pac["no_of_package"].ToString()),
                                        PackageDuration = ToNullableDecimal(pac["package_duration"].ToString()),
                                        LeadMinimumShare = ToNullableInt(pac["lead_minimum_share"].ToString()),
                                        JvMinShare = ToNullableInt(pac["jv_minimum_share"].ToString()),
                                        PackageName = pac["package_name"].ToString(),
                                        PackageValue = ToNullableDecimal(pac["package_value"].ToString()),
                                        Index = ToNullableInt(pac["index"].ToString()),
                                        PackageDisplayId = pac["package_disp_id"].ToString(),
                                        EstimateValue = ToNullableDecimal(pac["estimated_value"].ToString()),
                                        AverageRequirement = ToNullableDecimal(pac["average_requirement"].ToString()),
                                        CashFlowRequirement = ToNullableDecimal(pac["cash_flow_requirement"].ToString()),
                                    }
                                ).ToList();

                        context.BobEProcure_Packages.AddRange(ToInsert);
                        #endregion
                    }
                    catch(Exception ex)
                    {
                        new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                        {
                            FullMessage = ex.ToString(),
                            Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                            ServiceName = "PackageSync.BobEProcureConnector",
                        });
                        exception += ex.Message;
                    }
                    try
                    {
                        context.SaveChanges();
                    }
                    catch(Exception ex)
                    {
                        new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                        {
                            FullMessage = ex.ToString(),
                            Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                            ServiceName = "PackageSync.BobEProcureConnector",
                        });
                        exception += ex.Message;
                    }
                }
                #endregion
            }
            return exception == "" ? true : false;
        }
        public bool VendorSync(out string exception)
        {
            DataSet ds = new DataSet();
            exception = "";
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                conn.Open();
                string sql = "SELECT user_id, user_name, first_name, vendor_id, mobile, logged_status, user_status, company_id, company_name, company_registration_no, director_name, pq_cmp_address, pq_cmp_email, pq_cmp_mobile, pq_cmp_website, pq_cmp_pan, company_short_name, cin_no FROM \"lmp-dev\".v_vendor_master; ";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                da.Fill(ds);
                conn.Close();
            }
            catch (Exception msg)
            {
                new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                {
                    FullMessage = msg.ToString(),
                    Message = string.Format("Error occured while fetching data. Error: {0}", msg.Message),
                    ServiceName = "VendorSync.BobEProcureConnector",
                });
                exception += msg.Message;
            }
            if (ds.Tables.Count > 0)
            {
                #region Insert Or Update Records
                using (AMPEntities context = new AMPEntities())
                {
                    try
                    {
                        #region Update
                        var ToUpdate = (
                                    from ven in ds.Tables[0].AsEnumerable()
                                    join v in context.BobEProcure_Vendors on ven["user_id"].ToString() equals v.UserId
                                    where (ven["user_name"].ToString() != v.UserName) || (ven["first_name"].ToString() != v.FirstName) || (ven["vendor_id"].ToString() != v.VendorId) ||
                                          (ven["mobile"].ToString() != v.Mobile) || (ToNullableInt(ven["logged_status"].ToString()) != v.LoggedStatus) || (ToNullableInt(ven["user_status"].ToString()) != v.UserStatus) ||
                                          (ven["company_id"].ToString() != v.CompanyId) || (ven["company_name"].ToString() != v.CompanyName) || (ven["company_registration_no"].ToString() != v.CompanyRegistrationNo) ||
                                          (ven["director_name"].ToString() != v.DirectorName) || (ven["pq_cmp_address"].ToString() != v.PQCmpAddress) || (ven["pq_cmp_email"].ToString() != v.PQCmpEmail) ||
                                          (ven["pq_cmp_mobile"].ToString() != v.PQCmpMobile) || (ven["pq_cmp_website"].ToString() != v.PQCmpWebsite) || (ven["pq_cmp_pan"].ToString() != v.PQCmpPan) || (ven["company_short_name"].ToString() != v.CompanyShortName) ||
                                          (ven["cin_no"].ToString() != v.cin_no)
                                    select new VendorMaster()
                                    {
                                        Id = v.Id,
                                        UserName = ven["user_name"].ToString(),
                                        FirstName = ven["first_name"].ToString(),
                                        VendorId = ven["vendor_id"].ToString(),
                                        Mobile = ven["mobile"].ToString(),
                                        LoggedStatus = ToNullableInt(ven["logged_status"].ToString()),
                                        UserStatus = ToNullableInt(ven["user_status"].ToString()),
                                        CompanyId = ven["company_id"].ToString(),
                                        CompanyName = ven["company_name"].ToString(),
                                        CompanyRegistrationNo = ven["company_registration_no"].ToString(),
                                        DirectorName = ven["director_name"].ToString(),
                                        PQCmpAddress = ven["pq_cmp_address"].ToString(),
                                        PQCmpEmail = ven["pq_cmp_email"].ToString(),
                                        PQCmpMobile = ven["pq_cmp_mobile"].ToString(),
                                        PQCmpWebsite = ven["pq_cmp_website"].ToString(),
                                        PQCmpPan = ven["pq_cmp_pan"].ToString(),
                                        CompanyShortName = ven["company_short_name"].ToString(),
                                        cin_no = ven["cin_no"].ToString()
                                    }
                                ).ToList();
                        foreach (var record in ToUpdate)
                        {
                            var data = context.BobEProcure_Vendors.FirstOrDefault(e => e.Id == record.Id);
                            if (data != null)
                            {
                                data.UserName = record.UserName;
                                data.FirstName = record.FirstName;
                                data.VendorId = record.VendorId;
                                data.Mobile = record.Mobile;
                                data.LoggedStatus = record.LoggedStatus;
                                data.UserStatus = record.UserStatus;
                                data.CompanyId = record.CompanyId;
                                data.CompanyName = record.CompanyName;
                                data.CompanyRegistrationNo = record.CompanyRegistrationNo;
                                data.DirectorName = record.DirectorName;
                                data.PQCmpAddress = record.PQCmpAddress;
                                data.PQCmpEmail = record.PQCmpEmail;
                                data.PQCmpMobile = record.PQCmpMobile;
                                data.PQCmpWebsite = record.PQCmpWebsite;
                                data.PQCmpPan = record.PQCmpPan;
                                data.CompanyShortName = record.CompanyShortName;
                                data.cin_no = record.cin_no;
                            }
                        }
                        #endregion
                    }
                    catch(Exception ex)
                    {
                        new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                        {
                            FullMessage = ex.ToString(),
                            Message = string.Format("Error occured while paring for update. Error: {0}", ex.Message),
                            ServiceName = "VendorSync.BobEProcureConnector",
                        });
                        exception += ex.Message;
                    }

                    try
                    {
                        #region Insert
                        var ToInsert = (
                                    from ven in ds.Tables[0].AsEnumerable()
                                    join v in context.BobEProcure_Vendors on ven["user_id"].ToString() equals v.UserId into recs
                                    from rt in recs.DefaultIfEmpty()
                                    where rt == null
                                    select new BobEProcure_Vendors()
                                    {
                                        UserId = ven["user_id"].ToString(),
                                        UserName = ven["user_name"].ToString(),
                                        FirstName = ven["first_name"].ToString(),
                                        VendorId = ven["vendor_id"].ToString(),
                                        Mobile = ven["mobile"].ToString(),
                                        LoggedStatus = ToNullableInt(ven["logged_status"].ToString()),
                                        UserStatus = ToNullableInt(ven["user_status"].ToString()),
                                        CompanyId = ven["company_id"].ToString(),
                                        CompanyName = ven["company_name"].ToString(),
                                        CompanyRegistrationNo = ven["company_registration_no"].ToString(),
                                        DirectorName = ven["director_name"].ToString(),
                                        PQCmpAddress = ven["pq_cmp_address"].ToString(),
                                        PQCmpEmail = ven["pq_cmp_email"].ToString(),
                                        PQCmpMobile = ven["pq_cmp_mobile"].ToString(),
                                        PQCmpWebsite = ven["pq_cmp_website"].ToString(),
                                        PQCmpPan = ven["pq_cmp_pan"].ToString(),
                                        CompanyShortName = ven["company_short_name"].ToString(),
                                        cin_no = ven["cin_no"].ToString()
                                    }
                                ).ToList();

                        context.BobEProcure_Vendors.AddRange(ToInsert);
                        #endregion
                    }
                    catch(Exception ex)
                    {
                        new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                        {
                            FullMessage = ex.ToString(),
                            Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                            ServiceName = "VendorSync.BobEProcureConnector",
                        });
                        exception += ex.Message;
                    }

                    try
                    {
                        context.SaveChanges();
                    }
                    catch(Exception ex)
                    {
                        new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                        {
                            FullMessage = ex.ToString(),
                            Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                            ServiceName = "VendorSync.BobEProcureConnector",
                        });
                        exception += ex.Message;
                    }

                }
                #endregion
            }
            return exception == "" ? true : false;
        }
        public bool StatusSync(out string exception)
        {
            DataSet ds = new DataSet();
            exception = "";
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                conn.Open();
                string sql = "SELECT pq_supplier_id, pq_id, supplier_id, participation_status, participation_date, enc_status, supp_pq_subm_id, status, draft_status, submision_date, applicant_type, supplier_appr_dtl_id, prequalification_status, comments, pq_no FROM \"lmp-dev\".v_preq_status; ";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                da.Fill(ds);
                conn.Close();
            }
            catch (Exception msg)
            {
                new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                {
                    FullMessage = msg.ToString(),
                    Message = string.Format("Error occured while fetching data. Error: {0}", msg.Message),
                    ServiceName = "StatusSync.BobEProcureConnector",
                });
                exception += msg.Message;
            }
            if (ds.Tables.Count > 0)
            {
                #region Insert Or Update Records
                using (AMPEntities context = new AMPEntities())
                {
                    try
                    {
                        #region Update
                        var ToUpdate = (
                                    from sta in ds.Tables[0].AsEnumerable()
                                    join s in context.BobEProcure_Status on sta["pq_supplier_id"].ToString() equals s.PQSupplierId
                                    where (sta["pq_id"].ToString() != s.PQId) || (sta["supplier_id"].ToString() != s.SupplierId) || (ToNullableInt(sta["participation_status"].ToString()) != s.ParticipationStatus) ||
                                          (ToNullableDateTime(sta["participation_date"].ToString()) != s.ParticipationDate) || (ToNullableInt(sta["enc_status"].ToString()) != s.ENCStatus) ||
                                          (s.SuppPQSubmId != sta["supp_pq_subm_id"].ToString()) || (s.Status != ToNullableInt(sta["status"].ToString())) || (s.DraftStatus != ToNullableInt(sta["draft_status"].ToString())) ||
                                          (s.SubmisionDate != ToNullableDateTime(sta["submision_date"].ToString())) || (s.ApplicantType != sta["applicant_type"].ToString()) ||
                                          (s.SupplierApprDtlId != sta["supplier_appr_dtl_id"].ToString()) || (s.PreQualificationStatus != ToNullableInt(sta["prequalification_status"].ToString())) || (s.Comments != sta["comments"].ToString()) ||
                                          (s.PQNo != sta["pq_no"].ToString())

                                    select new PQStatus()
                                    {
                                        Id = s.Id,
                                        PQId = sta["pq_id"].ToString(),
                                        SupplierId = sta["supplier_id"].ToString(),
                                        ParticipationStatus = ToNullableInt(sta["participation_status"].ToString()),
                                        ParticipationDate = ToNullableDateTime(sta["participation_date"].ToString()),
                                        ENCStatus = ToNullableInt(sta["enc_status"].ToString()),
                                        SuppPQSubmId = sta["supp_pq_subm_id"].ToString(),
                                        Status = ToNullableInt(sta["status"].ToString()),
                                        DraftStatus = ToNullableInt(sta["draft_status"].ToString()),
                                        SubmisionDate = ToNullableDateTime(sta["submision_date"].ToString()),
                                        ApplicantType = sta["applicant_type"].ToString(),
                                        SupplierApprDtlId = sta["supplier_appr_dtl_id"].ToString(),
                                        PreQualificationStatus = ToNullableInt(sta["prequalification_status"].ToString()),
                                        Comments = sta["comments"].ToString(),
                                        PQNo = sta["pq_no"].ToString()
                                    }
                                ).ToList();

                        foreach (var record in ToUpdate)
                        {
                            var data = context.BobEProcure_Status.FirstOrDefault(e => e.Id == record.Id);
                            if (data != null)
                            {
                                data.PQId = record.PQId;
                                data.SupplierId = record.PQSupplierId;
                                data.ParticipationStatus = record.ParticipationStatus;
                                data.ParticipationDate = record.ParticipationDate;
                                data.ENCStatus = record.ENCStatus;
                                data.SuppPQSubmId = record.SuppPQSubmId;
                                data.Status = record.Status;
                                data.DraftStatus = record.DraftStatus;
                                data.SubmisionDate = record.SubmisionDate;
                                data.ApplicantType = record.ApplicantType;
                                data.SupplierApprDtlId = record.SupplierApprDtlId;
                                data.PreQualificationStatus = record.PreQualificationStatus;
                                data.Comments = record.Comments;
                                data.PQNo = record.PQNo;
                            }
                        }
                        #endregion
                    }
                    catch(Exception ex)
                    {
                        new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                        {
                            FullMessage = ex.ToString(),
                            Message = string.Format("Error occured while paring for update. Error: {0}", ex.Message),
                            ServiceName = "StatusSync.BobEProcureConnector",
                        });
                        exception += ex.Message;
                    }

                    try
                    {
                        #region Insert
                        var ToInsert = (
                                    from sta in ds.Tables[0].AsEnumerable()
                                    join s in context.BobEProcure_Status on sta["pq_supplier_id"].ToString() equals s.PQSupplierId into recs
                                    from rt in recs.DefaultIfEmpty()
                                    where rt == null
                                    select new BobEProcure_Status()
                                    {
                                        PQId = sta["pq_id"].ToString(),
                                        PQSupplierId = sta["pq_supplier_id"].ToString(),
                                        SupplierId = sta["supplier_id"].ToString(),
                                        ParticipationStatus = ToNullableInt(sta["participation_status"].ToString()),
                                        ParticipationDate = ToNullableDateTime(sta["participation_date"].ToString()),
                                        ENCStatus = ToNullableInt(sta["enc_status"].ToString()),
                                        SuppPQSubmId = sta["supp_pq_subm_id"].ToString(),
                                        Status = ToNullableInt(sta["status"].ToString()),
                                        DraftStatus = ToNullableInt(sta["draft_status"].ToString()),
                                        SubmisionDate = ToNullableDateTime(sta["submision_date"].ToString()),
                                        ApplicantType = sta["applicant_type"].ToString(),
                                        SupplierApprDtlId = sta["supplier_appr_dtl_id"].ToString(),
                                        PreQualificationStatus = ToNullableInt(sta["prequalification_status"].ToString()),
                                        Comments = sta["comments"].ToString(),
                                        PQNo = sta["pq_no"].ToString()
                                    }
                                ).ToList();

                        context.BobEProcure_Status.AddRange(ToInsert);
                        #endregion
                    }
                    catch(Exception ex)
                    {
                        new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                        {
                            FullMessage = ex.ToString(),
                            Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                            ServiceName = "StatusSync.BobEProcureConnector",
                        });
                        exception += ex.Message;
                    }

                    try
                    {
                        context.SaveChanges();
                    }
                    catch(Exception ex)
                    {
                        new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                        {
                            FullMessage = ex.ToString(),
                            Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                            ServiceName = "StatusSync.BobEProcureConnector",
                        });
                        exception += ex.Message;
                    }
                }
                #endregion
            }
            return exception == "" ? true : false;
        }
    }
}