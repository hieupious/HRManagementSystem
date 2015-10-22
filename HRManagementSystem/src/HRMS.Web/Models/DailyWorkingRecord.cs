using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace HRMS.Web.Models
{
    public class DailyWorkingRecord
    {
        public int Id { get; set; }
        public int? UserInfoId { get; set; }
        public int? MonthlyRecordId { get; set; }
        public DateTime WorkingDay { get; set; }
        public DateTime? CheckIn
        {
            get
            {
                if (CheckInOutRecords != null)
                {
                    var en = CheckInOutRecords.GetEnumerator();
                    if(en.MoveNext())
                    {
                        var checkIn = en.Current.CheckTime;
                        while (en.MoveNext())
                        {
                            if (en.Current.CheckTime < checkIn)
                                checkIn = en.Current.CheckTime;
                        }
                        return checkIn;
                    }
                }
                return null;
            }
            set { }
        }
        public DateTime? CheckOut
        {
            get
            {
                if (CheckInOutRecords != null)
                {
                    var en = CheckInOutRecords.GetEnumerator();
                    if(en.MoveNext())
                    {
                        var checkOut = en.Current.CheckTime;
                        while (en.MoveNext())
                        {
                            if (en.Current.CheckTime > checkOut)
                                checkOut = en.Current.CheckTime;
                        }
                        return checkOut;
                    }
                }
                return null;
            }
            set { }
        }
        public double MinuteLate { get; set; }
        public string GetApprovedReason { get; set; }
        public int? ApproverId { get; set; }
        public string ApproverComment { get; set; }
        public bool? Approved { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual UserInfo Approver { get; set; }
        public virtual ICollection<CheckInOutRecord> CheckInOutRecords { get; set; }
        [JsonIgnore]
        public virtual MonthlyRecord MonthlyRecord { get; set; }


    }
}
