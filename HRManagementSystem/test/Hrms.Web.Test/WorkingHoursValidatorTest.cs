using HRMS.Web.Models;
using HRMS.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Hrms.Web.Test
{
    public class WorkingHoursValidatorTest
    {
        #region Standard

        #region WorkingHoursRules

        public static WorkingHoursRuleBase _standard_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
        {
            WorkingTimeStart = new TimeSpan(8, 0, 0),
            WorkingTimeEnd = new TimeSpan(17, 0, 0),
            BreaktimeStart = new TimeSpan(12, 0, 0),
            BreaktimeEnd = new TimeSpan(13, 0, 0)
        };

        public static WorkingPoliciesGroup _standard_Group = new WorkingPoliciesGroup()
        {
            WorkingHoursRules = new WorkingHoursRuleBase[]
            {
                _standard_BaseTimeWorkingHoursRule
            }
        };

        #endregion

        [Theory]
        #region InlineData
        [InlineData(7, 50, 7, 50, 480)]
        [InlineData(7, 50, 8, 0, 480)]
        [InlineData(7, 50, 8, 10, 470)]
        [InlineData(7, 50, 11, 50, 250)]
        [InlineData(7, 50, 12, 0, 240)]
        [InlineData(7, 50, 12, 30, 240)]
        [InlineData(7, 50, 13, 0, 240)]
        [InlineData(7, 50, 13, 10, 230)]
        [InlineData(7, 50, 16, 50, 10)]
        [InlineData(7, 50, 17, 0, 0)]
        [InlineData(7, 50, 17, 10, 0)]
        [InlineData(8, 0, 8, 10, 470)]
        [InlineData(8, 0, 11, 50, 250)]
        [InlineData(8, 0, 12, 0, 240)]
        [InlineData(8, 0, 12, 30, 240)]
        [InlineData(8, 0, 13, 0, 240)]
        [InlineData(8, 0, 13, 10, 230)]
        [InlineData(8, 0, 16, 50, 10)]
        [InlineData(8, 0, 17, 0, 0)]
        [InlineData(8, 0, 17, 10, 0)]
        [InlineData(8, 10, 11, 50, 260)]
        [InlineData(8, 10, 12, 0, 250)]
        [InlineData(8, 10, 12, 30, 250)]
        [InlineData(8, 10, 13, 0, 250)]
        [InlineData(8, 10, 13, 10, 240)]
        [InlineData(8, 10, 16, 50, 20)]
        [InlineData(8, 10, 17, 0, 10)]
        [InlineData(8, 10, 17, 10, 10)]
        [InlineData(11, 50, 12, 0, 470)]
        [InlineData(11, 50, 12, 30, 470)]
        [InlineData(11, 50, 13, 0, 470)]
        [InlineData(11, 50, 13, 10, 460)]
        [InlineData(11, 50, 16, 50, 240)]
        [InlineData(11, 50, 17, 0, 230)]
        [InlineData(11, 50, 17, 10, 230)]
        [InlineData(12, 0, 12, 30, 480)]
        [InlineData(12, 0, 13, 0, 480)]
        [InlineData(12, 0, 13, 10, 470)]
        [InlineData(12, 0, 16, 50, 250)]
        [InlineData(12, 0, 17, 0, 240)]
        [InlineData(12, 0, 17, 10, 240)]
        [InlineData(12, 30, 13, 0, 480)]
        [InlineData(12, 30, 13, 10, 470)]
        [InlineData(12, 30, 16, 50, 250)]
        [InlineData(12, 30, 17, 0, 240)]
        [InlineData(12, 30, 17, 10, 240)]
        [InlineData(13, 0, 13, 10, 470)]
        [InlineData(13, 0, 16, 50, 250)]
        [InlineData(13, 0, 17, 0, 240)]
        [InlineData(13, 0, 17, 10, 240)]
        [InlineData(13, 10, 16, 50, 260)]
        [InlineData(13, 10, 17, 0, 250)]
        [InlineData(13, 10, 17, 10, 250)]
        [InlineData(16, 50, 17, 0, 470)]
        [InlineData(16, 50, 17, 10, 470)]
        [InlineData(17, 0, 17, 10, 480)]
        #endregion
        public void WorkingHoursValidator_Standard_LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime)
        {
            LackingTime(checkInHour, checkInMinute, checkOutHour, checkOutMinute, lackingTime, _standard_Group);
        }

        #endregion

        #region Standard Maternity Arrive Late

        #region WorkingHoursRules

        public static WorkingHoursRuleBase _standard_ml_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
        {
            WorkingTimeStart = new TimeSpan(9, 0, 0),
            WorkingTimeEnd = new TimeSpan(17, 0, 0),
            BreaktimeStart = new TimeSpan(12, 0, 0),
            BreaktimeEnd = new TimeSpan(13, 0, 0)
        };

        public static WorkingPoliciesGroup _standard_ml_Group = new WorkingPoliciesGroup()
        {
            WorkingHoursRules = new WorkingHoursRuleBase[]
            {
                _standard_ml_BaseTimeWorkingHoursRule
            }
        };

        #endregion

        [Theory]
        #region InlineData
        [InlineData(8, 50, 9, 0, 420)]
        [InlineData(8, 50, 9, 10, 410)]
        [InlineData(8, 50, 11, 50, 250)]
        [InlineData(8, 50, 12, 0, 240)]
        [InlineData(8, 50, 12, 30, 240)]
        [InlineData(8, 50, 13, 0, 240)]
        [InlineData(8, 50, 13, 10, 230)]
        [InlineData(8, 50, 16, 50, 10)]
        [InlineData(8, 50, 17, 0, 0)]
        [InlineData(8, 50, 17, 10, 0)]
        [InlineData(9, 0, 9, 10, 410)]
        [InlineData(9, 0, 11, 50, 250)]
        [InlineData(9, 0, 12, 0, 240)]
        [InlineData(9, 0, 12, 30, 240)]
        [InlineData(9, 0, 13, 0, 240)]
        [InlineData(9, 0, 13, 10, 230)]
        [InlineData(9, 0, 16, 50, 10)]
        [InlineData(9, 0, 17, 0, 0)]
        [InlineData(9, 0, 17, 10, 0)]
        [InlineData(9, 10, 11, 50, 260)]
        [InlineData(9, 10, 12, 0, 250)]
        [InlineData(9, 10, 12, 30, 250)]
        [InlineData(9, 10, 13, 0, 250)]
        [InlineData(9, 10, 13, 10, 240)]
        [InlineData(9, 10, 16, 50, 20)]
        [InlineData(9, 10, 17, 0, 10)]
        [InlineData(9, 10, 17, 10, 10)]
        [InlineData(11, 50, 12, 0, 410)]
        [InlineData(11, 50, 12, 30, 410)]
        [InlineData(11, 50, 13, 0, 410)]
        [InlineData(11, 50, 13, 10, 400)]
        [InlineData(11, 50, 16, 50, 180)]
        [InlineData(11, 50, 17, 0, 170)]
        [InlineData(11, 50, 17, 10, 170)]
        [InlineData(12, 0, 12, 30, 420)]
        [InlineData(12, 0, 13, 0, 420)]
        [InlineData(12, 0, 13, 10, 410)]
        [InlineData(12, 0, 16, 50, 190)]
        [InlineData(12, 0, 17, 0, 180)]
        [InlineData(12, 0, 17, 10, 180)]
        [InlineData(12, 30, 13, 0, 420)]
        [InlineData(12, 30, 13, 10, 410)]
        [InlineData(12, 30, 16, 50, 190)]
        [InlineData(12, 30, 17, 0, 180)]
        [InlineData(12, 30, 17, 10, 180)]
        [InlineData(13, 0, 13, 10, 410)]
        [InlineData(13, 0, 16, 50, 190)]
        [InlineData(13, 0, 17, 0, 180)]
        [InlineData(13, 0, 17, 10, 180)]
        [InlineData(13, 10, 16, 50, 200)]
        [InlineData(13, 10, 17, 0, 190)]
        [InlineData(13, 10, 17, 10, 190)]
        [InlineData(16, 50, 17, 0, 410)]
        [InlineData(16, 50, 17, 10, 410)]
        [InlineData(17, 0, 17, 10, 420)]
        #endregion
        public void WorkingHoursValidator_Standard_ML_LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime)
        {
            LackingTime(checkInHour, checkInMinute, checkOutHour, checkOutMinute, lackingTime, _standard_ml_Group);
        }

        #endregion

        #region Standard Maternity Arrive Late

        #region WorkingHoursRules

        public static WorkingHoursRuleBase _standard_me_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
        {
            WorkingTimeStart = new TimeSpan(8, 0, 0),
            WorkingTimeEnd = new TimeSpan(16, 0, 0),
            BreaktimeStart = new TimeSpan(12, 0, 0),
            BreaktimeEnd = new TimeSpan(13, 0, 0)
        };

        public static WorkingPoliciesGroup _standard_me_Group = new WorkingPoliciesGroup()
        {
            WorkingHoursRules = new WorkingHoursRuleBase[]
            {
                _standard_me_BaseTimeWorkingHoursRule
            }
        };

        #endregion

        [Theory]
        #region InlineData
        [InlineData(7, 50, 8, 0, 420)]
        [InlineData(7, 50, 8, 10, 410)]
        [InlineData(7, 50, 11, 50, 190)]
        [InlineData(7, 50, 12, 0, 180)]
        [InlineData(7, 50, 12, 30, 180)]
        [InlineData(7, 50, 13, 0, 180)]
        [InlineData(7, 50, 13, 10, 170)]
        [InlineData(7, 50, 15, 50, 10)]
        [InlineData(7, 50, 16, 0, 0)]
        [InlineData(7, 50, 16, 10, 0)]
        [InlineData(8, 0, 8, 10, 410)]
        [InlineData(8, 0, 11, 50, 190)]
        [InlineData(8, 0, 12, 0, 180)]
        [InlineData(8, 0, 12, 30, 180)]
        [InlineData(8, 0, 13, 0, 180)]
        [InlineData(8, 0, 13, 10, 170)]
        [InlineData(8, 0, 15, 50, 10)]
        [InlineData(8, 0, 16, 0, 0)]
        [InlineData(8, 0, 16, 10, 0)]
        [InlineData(8, 10, 11, 50, 200)]
        [InlineData(8, 10, 12, 0, 190)]
        [InlineData(8, 10, 12, 30, 190)]
        [InlineData(8, 10, 13, 0, 190)]
        [InlineData(8, 10, 13, 10, 180)]
        [InlineData(8, 10, 15, 50, 20)]
        [InlineData(8, 10, 16, 0, 10)]
        [InlineData(8, 10, 16, 10, 10)]
        [InlineData(11, 50, 12, 0, 410)]
        [InlineData(11, 50, 12, 30, 410)]
        [InlineData(11, 50, 13, 0, 410)]
        [InlineData(11, 50, 13, 10, 400)]
        [InlineData(11, 50, 15, 50, 240)]
        [InlineData(11, 50, 16, 0, 230)]
        [InlineData(11, 50, 16, 10, 230)]
        [InlineData(12, 0, 12, 30, 420)]
        [InlineData(12, 0, 13, 0, 420)]
        [InlineData(12, 0, 13, 10, 410)]
        [InlineData(12, 0, 15, 50, 250)]
        [InlineData(12, 0, 16, 0, 240)]
        [InlineData(12, 0, 16, 10, 240)]
        [InlineData(12, 30, 13, 0, 420)]
        [InlineData(12, 30, 13, 10, 410)]
        [InlineData(12, 30, 15, 50, 250)]
        [InlineData(12, 30, 16, 0, 240)]
        [InlineData(12, 30, 16, 10, 240)]
        [InlineData(13, 0, 13, 10, 410)]
        [InlineData(13, 0, 15, 50, 250)]
        [InlineData(13, 0, 16, 0, 240)]
        [InlineData(13, 0, 16, 10, 240)]
        [InlineData(13, 10, 15, 50, 260)]
        [InlineData(13, 10, 16, 0, 250)]
        [InlineData(13, 10, 16, 10, 250)]
        [InlineData(15, 50, 16, 0, 410)]
        [InlineData(15, 50, 16, 10, 410)]
        [InlineData(16, 0, 16, 10, 420)]
        #endregion
        public void WorkingHoursValidator_Standard_ME_LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime)
        {
            LackingTime(checkInHour, checkInMinute, checkOutHour, checkOutMinute, lackingTime, _standard_me_Group);
        }

        #endregion

        #region Devs / QAs

        #region WorkingHoursRules

        public static WorkingHoursRuleBase _devsqas_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
        {
            WorkingTimeStart = new TimeSpan(8, 0, 0),
            WorkingTimeEnd = new TimeSpan(17, 0, 0),
            BreaktimeStart = new TimeSpan(11, 45, 0),
            BreaktimeEnd = new TimeSpan(12, 45, 0)
        };

        public static WorkingHoursRuleBase _devsqas_EarlyToleranceWorkingHoursRule = new EarlyToleranceWorkingHoursRule()
        {
            Tolerance = new TimeSpan(1, 0, 0)
        };

        public static WorkingHoursRuleBase _devsqas_LateToleranceWorkingHoursRule = new LateToleranceWorkingHoursRule()
        {
            Tolerance = new TimeSpan(1, 0, 0)
        };

        public static WorkingPoliciesGroup _devsqas_Group = new WorkingPoliciesGroup()
        {
            WorkingHoursRules = new WorkingHoursRuleBase[]
            {
                _devsqas_BaseTimeWorkingHoursRule,
                _devsqas_EarlyToleranceWorkingHoursRule,
                _devsqas_LateToleranceWorkingHoursRule
            }
        };

        #endregion

        [Theory]
        #region InlineData
        [InlineData(6, 50, 7, 0, 480)]
        [InlineData(6, 50, 7, 30, 450)]
        [InlineData(6, 50, 8, 0, 420)]
        [InlineData(6, 50, 8, 30, 390)]
        [InlineData(6, 50, 9, 0, 360)]
        [InlineData(6, 50, 9, 10, 350)]
        [InlineData(6, 50, 11, 35, 205)]
        [InlineData(6, 50, 11, 45, 195)]
        [InlineData(6, 50, 12, 10, 195)]
        [InlineData(6, 50, 12, 45, 195)]
        [InlineData(6, 50, 12, 55, 185)]
        [InlineData(6, 50, 15, 50, 10)]
        [InlineData(6, 50, 16, 0, 0)]
        [InlineData(6, 50, 16, 30, 0)]
        [InlineData(6, 50, 17, 0, 0)]
        [InlineData(6, 50, 17, 30, 0)]
        [InlineData(6, 50, 18, 0, 0)]
        [InlineData(6, 50, 18, 10, 0)]
        [InlineData(7, 0, 7, 30, 450)]
        [InlineData(7, 0, 8, 0, 420)]
        [InlineData(7, 0, 8, 30, 390)]
        [InlineData(7, 0, 9, 0, 360)]
        [InlineData(7, 0, 9, 10, 350)]
        [InlineData(7, 0, 11, 35, 205)]
        [InlineData(7, 0, 11, 45, 195)]
        [InlineData(7, 0, 12, 10, 195)]
        [InlineData(7, 0, 12, 45, 195)]
        [InlineData(7, 0, 12, 55, 185)]
        [InlineData(7, 0, 15, 50, 10)]
        [InlineData(7, 0, 16, 0, 0)]
        [InlineData(7, 0, 16, 30, 0)]
        [InlineData(7, 0, 17, 0, 0)]
        [InlineData(7, 0, 17, 30, 0)]
        [InlineData(7, 0, 18, 0, 0)]
        [InlineData(7, 0, 18, 10, 0)]
        [InlineData(7, 30, 8, 0, 450)]
        [InlineData(7, 30, 8, 30, 420)]
        [InlineData(7, 30, 9, 0, 390)]
        [InlineData(7, 30, 9, 10, 380)]
        [InlineData(7, 30, 11, 35, 235)]
        [InlineData(7, 30, 11, 45, 225)]
        [InlineData(7, 30, 12, 10, 225)]
        [InlineData(7, 30, 12, 45, 225)]
        [InlineData(7, 30, 12, 55, 215)]
        [InlineData(7, 30, 15, 50, 40)]
        [InlineData(7, 30, 16, 0, 30)]
        [InlineData(7, 30, 16, 30, 0)]
        [InlineData(7, 30, 17, 0, 0)]
        [InlineData(7, 30, 17, 30, 0)]
        [InlineData(7, 30, 18, 0, 0)]
        [InlineData(7, 30, 18, 10, 0)]
        [InlineData(8, 0, 8, 30, 450)]
        [InlineData(8, 0, 9, 0, 420)]
        [InlineData(8, 0, 9, 10, 410)]
        [InlineData(8, 0, 11, 35, 265)]
        [InlineData(8, 0, 11, 45, 255)]
        [InlineData(8, 0, 12, 10, 255)]
        [InlineData(8, 0, 12, 45, 255)]
        [InlineData(8, 0, 12, 55, 245)]
        [InlineData(8, 0, 15, 50, 70)]
        [InlineData(8, 0, 16, 0, 60)]
        [InlineData(8, 0, 16, 30, 30)]
        [InlineData(8, 0, 17, 0, 0)]
        [InlineData(8, 0, 17, 30, 0)]
        [InlineData(8, 0, 18, 0, 0)]
        [InlineData(8, 0, 18, 10, 0)]
        [InlineData(8, 30, 9, 0, 450)]
        [InlineData(8, 30, 9, 10, 440)]
        [InlineData(8, 30, 11, 35, 295)]
        [InlineData(8, 30, 11, 45, 285)]
        [InlineData(8, 30, 12, 10, 285)]
        [InlineData(8, 30, 12, 45, 285)]
        [InlineData(8, 30, 12, 55, 275)]
        [InlineData(8, 30, 15, 50, 100)]
        [InlineData(8, 30, 16, 0, 90)]
        [InlineData(8, 30, 16, 30, 60)]
        [InlineData(8, 30, 17, 0, 30)]
        [InlineData(8, 30, 17, 30, 0)]
        [InlineData(8, 30, 18, 0, 0)]
        [InlineData(8, 30, 18, 10, 0)]
        [InlineData(9, 0, 9, 10, 470)]
        [InlineData(9, 0, 11, 35, 325)]
        [InlineData(9, 0, 11, 45, 315)]
        [InlineData(9, 0, 12, 10, 315)]
        [InlineData(9, 0, 12, 45, 315)]
        [InlineData(9, 0, 12, 55, 305)]
        [InlineData(9, 0, 15, 50, 130)]
        [InlineData(9, 0, 16, 0, 120)]
        [InlineData(9, 0, 16, 30, 90)]
        [InlineData(9, 0, 17, 0, 60)]
        [InlineData(9, 0, 17, 30, 30)]
        [InlineData(9, 0, 18, 0, 0)]
        [InlineData(9, 0, 18, 10, 0)]
        [InlineData(9, 10, 11, 35, 335)]
        [InlineData(9, 10, 11, 45, 325)]
        [InlineData(9, 10, 12, 10, 325)]
        [InlineData(9, 10, 12, 45, 325)]
        [InlineData(9, 10, 12, 55, 315)]
        [InlineData(9, 10, 15, 50, 140)]
        [InlineData(9, 10, 16, 0, 130)]
        [InlineData(9, 10, 16, 30, 100)]
        [InlineData(9, 10, 17, 0, 70)]
        [InlineData(9, 10, 17, 30, 40)]
        [InlineData(9, 10, 18, 0, 10)]
        [InlineData(9, 10, 18, 10, 10)]
        [InlineData(11, 35, 11, 45, 470)]
        [InlineData(11, 35, 12, 10, 470)]
        [InlineData(11, 35, 12, 45, 470)]
        [InlineData(11, 35, 12, 55, 460)]
        [InlineData(11, 35, 15, 50, 285)]
        [InlineData(11, 35, 16, 0, 275)]
        [InlineData(11, 35, 16, 30, 245)]
        [InlineData(11, 35, 17, 0, 215)]
        [InlineData(11, 35, 17, 30, 185)]
        [InlineData(11, 35, 18, 0, 155)]
        [InlineData(11, 35, 18, 10, 155)]
        [InlineData(11, 45, 12, 10, 480)]
        [InlineData(11, 45, 12, 45, 480)]
        [InlineData(11, 45, 12, 55, 470)]
        [InlineData(11, 45, 15, 50, 295)]
        [InlineData(11, 45, 16, 0, 285)]
        [InlineData(11, 45, 16, 30, 255)]
        [InlineData(11, 45, 17, 0, 225)]
        [InlineData(11, 45, 17, 30, 195)]
        [InlineData(11, 45, 18, 0, 165)]
        [InlineData(11, 45, 18, 10, 165)]
        [InlineData(12, 10, 12, 45, 480)]
        [InlineData(12, 10, 12, 55, 470)]
        [InlineData(12, 10, 15, 50, 295)]
        [InlineData(12, 10, 16, 0, 285)]
        [InlineData(12, 10, 16, 30, 255)]
        [InlineData(12, 10, 17, 0, 225)]
        [InlineData(12, 10, 17, 30, 195)]
        [InlineData(12, 10, 18, 0, 165)]
        [InlineData(12, 10, 18, 10, 165)]
        [InlineData(12, 45, 12, 55, 470)]
        [InlineData(12, 45, 15, 50, 295)]
        [InlineData(12, 45, 16, 0, 285)]
        [InlineData(12, 45, 16, 30, 255)]
        [InlineData(12, 45, 17, 0, 225)]
        [InlineData(12, 45, 17, 30, 195)]
        [InlineData(12, 45, 18, 0, 165)]
        [InlineData(12, 45, 18, 10, 165)]
        [InlineData(12, 55, 15, 50, 305)]
        [InlineData(12, 55, 16, 0, 295)]
        [InlineData(12, 55, 16, 30, 265)]
        [InlineData(12, 55, 17, 0, 235)]
        [InlineData(12, 55, 17, 30, 205)]
        [InlineData(12, 55, 18, 0, 175)]
        [InlineData(12, 55, 18, 10, 175)]
        [InlineData(15, 50, 16, 0, 470)]
        [InlineData(15, 50, 16, 30, 440)]
        [InlineData(15, 50, 17, 0, 410)]
        [InlineData(15, 50, 17, 30, 380)]
        [InlineData(15, 50, 18, 0, 350)]
        [InlineData(15, 50, 18, 10, 350)]
        [InlineData(16, 0, 16, 30, 450)]
        [InlineData(16, 0, 17, 0, 420)]
        [InlineData(16, 0, 17, 30, 390)]
        [InlineData(16, 0, 18, 0, 360)]
        [InlineData(16, 0, 18, 10, 360)]
        [InlineData(16, 30, 17, 0, 450)]
        [InlineData(16, 30, 17, 30, 420)]
        [InlineData(16, 30, 18, 0, 390)]
        [InlineData(16, 30, 18, 10, 390)]
        [InlineData(17, 0, 17, 30, 450)]
        [InlineData(17, 0, 18, 0, 420)]
        [InlineData(17, 0, 18, 10, 420)]
        [InlineData(17, 30, 18, 0, 450)]
        [InlineData(17, 30, 18, 10, 450)]
        [InlineData(18, 0, 18, 10, 480)]
        #endregion
        public void WorkingHoursValidator_DevsQAs_LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime)
        {
            LackingTime(checkInHour, checkInMinute, checkOutHour, checkOutMinute, lackingTime, _devsqas_Group);
        }

        #endregion

        #region Devs / QAs Maternity Arrive Late

        #region WorkingHoursRules

        public static WorkingHoursRuleBase _devsqas_ml_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
        {
            WorkingTimeStart = new TimeSpan(9, 0, 0),
            WorkingTimeEnd = new TimeSpan(17, 0, 0),
            BreaktimeStart = new TimeSpan(11, 45, 0),
            BreaktimeEnd = new TimeSpan(12, 45, 0)
        };

        public static WorkingHoursRuleBase _devsqas_ml_EarlyToleranceWorkingHoursRule = new EarlyToleranceWorkingHoursRule()
        {
            Tolerance = new TimeSpan(1, 0, 0)
        };

        public static WorkingHoursRuleBase _devsqas_ml_LateToleranceWorkingHoursRule = new LateToleranceWorkingHoursRule()
        {
            Tolerance = new TimeSpan(1, 0, 0)
        };

        public static WorkingPoliciesGroup _devsqas_ml_Group = new WorkingPoliciesGroup()
        {
            WorkingHoursRules = new WorkingHoursRuleBase[]
            {
                _devsqas_ml_BaseTimeWorkingHoursRule,
                _devsqas_ml_EarlyToleranceWorkingHoursRule,
                _devsqas_ml_LateToleranceWorkingHoursRule
            }
        };

        #endregion

        [Theory]
        #region InlineData
        [InlineData(7, 50, 8, 0, 420)]
        [InlineData(7, 50, 8, 30, 390)]
        [InlineData(7, 50, 9, 0, 360)]
        [InlineData(7, 50, 9, 30, 330)]
        [InlineData(7, 50, 10, 0, 300)]
        [InlineData(7, 50, 10, 10, 290)]
        [InlineData(7, 50, 11, 35, 205)]
        [InlineData(7, 50, 11, 45, 195)]
        [InlineData(7, 50, 12, 10, 195)]
        [InlineData(7, 50, 12, 45, 195)]
        [InlineData(7, 50, 12, 55, 185)]
        [InlineData(7, 50, 15, 50, 10)]
        [InlineData(7, 50, 16, 0, 0)]
        [InlineData(7, 50, 16, 30, 0)]
        [InlineData(7, 50, 17, 0, 0)]
        [InlineData(7, 50, 17, 30, 0)]
        [InlineData(7, 50, 18, 0, 0)]
        [InlineData(7, 50, 18, 10, 0)]
        [InlineData(8, 0, 8, 30, 390)]
        [InlineData(8, 0, 9, 0, 360)]
        [InlineData(8, 0, 9, 30, 330)]
        [InlineData(8, 0, 10, 0, 300)]
        [InlineData(8, 0, 10, 10, 290)]
        [InlineData(8, 0, 11, 35, 205)]
        [InlineData(8, 0, 11, 45, 195)]
        [InlineData(8, 0, 12, 10, 195)]
        [InlineData(8, 0, 12, 45, 195)]
        [InlineData(8, 0, 12, 55, 185)]
        [InlineData(8, 0, 15, 50, 10)]
        [InlineData(8, 0, 16, 0, 0)]
        [InlineData(8, 0, 16, 30, 0)]
        [InlineData(8, 0, 17, 0, 0)]
        [InlineData(8, 0, 17, 30, 0)]
        [InlineData(8, 0, 18, 0, 0)]
        [InlineData(8, 0, 18, 10, 0)]
        [InlineData(8, 30, 9, 0, 390)]
        [InlineData(8, 30, 9, 30, 360)]
        [InlineData(8, 30, 10, 0, 330)]
        [InlineData(8, 30, 10, 10, 320)]
        [InlineData(8, 30, 11, 35, 235)]
        [InlineData(8, 30, 11, 45, 225)]
        [InlineData(8, 30, 12, 10, 225)]
        [InlineData(8, 30, 12, 45, 225)]
        [InlineData(8, 30, 12, 55, 215)]
        [InlineData(8, 30, 15, 50, 40)]
        [InlineData(8, 30, 16, 0, 30)]
        [InlineData(8, 30, 16, 30, 0)]
        [InlineData(8, 30, 17, 0, 0)]
        [InlineData(8, 30, 17, 30, 0)]
        [InlineData(8, 30, 18, 0, 0)]
        [InlineData(8, 30, 18, 10, 0)]
        [InlineData(9, 0, 9, 30, 390)]
        [InlineData(9, 0, 10, 0, 360)]
        [InlineData(9, 0, 10, 10, 350)]
        [InlineData(9, 0, 11, 35, 265)]
        [InlineData(9, 0, 11, 45, 255)]
        [InlineData(9, 0, 12, 10, 255)]
        [InlineData(9, 0, 12, 45, 255)]
        [InlineData(9, 0, 12, 55, 245)]
        [InlineData(9, 0, 15, 50, 70)]
        [InlineData(9, 0, 16, 0, 60)]
        [InlineData(9, 0, 16, 30, 30)]
        [InlineData(9, 0, 17, 0, 0)]
        [InlineData(9, 0, 17, 30, 0)]
        [InlineData(9, 0, 18, 0, 0)]
        [InlineData(9, 0, 18, 10, 0)]
        [InlineData(9, 30, 10, 0, 390)]
        [InlineData(9, 30, 10, 10, 380)]
        [InlineData(9, 30, 11, 35, 295)]
        [InlineData(9, 30, 11, 45, 285)]
        [InlineData(9, 30, 12, 10, 285)]
        [InlineData(9, 30, 12, 45, 285)]
        [InlineData(9, 30, 12, 55, 275)]
        [InlineData(9, 30, 15, 50, 100)]
        [InlineData(9, 30, 16, 0, 90)]
        [InlineData(9, 30, 16, 30, 60)]
        [InlineData(9, 30, 17, 0, 30)]
        [InlineData(9, 30, 17, 30, 0)]
        [InlineData(9, 30, 18, 0, 0)]
        [InlineData(9, 30, 18, 10, 0)]
        [InlineData(10, 0, 10, 10, 410)]
        [InlineData(10, 0, 11, 35, 325)]
        [InlineData(10, 0, 11, 45, 315)]
        [InlineData(10, 0, 12, 10, 315)]
        [InlineData(10, 0, 12, 45, 315)]
        [InlineData(10, 0, 12, 55, 305)]
        [InlineData(10, 0, 15, 50, 130)]
        [InlineData(10, 0, 16, 0, 120)]
        [InlineData(10, 0, 16, 30, 90)]
        [InlineData(10, 0, 17, 0, 60)]
        [InlineData(10, 0, 17, 30, 30)]
        [InlineData(10, 0, 18, 0, 0)]
        [InlineData(10, 0, 18, 10, 0)]
        [InlineData(10, 10, 11, 35, 335)]
        [InlineData(10, 10, 11, 45, 325)]
        [InlineData(10, 10, 12, 10, 325)]
        [InlineData(10, 10, 12, 45, 325)]
        [InlineData(10, 10, 12, 55, 315)]
        [InlineData(10, 10, 15, 50, 140)]
        [InlineData(10, 10, 16, 0, 130)]
        [InlineData(10, 10, 16, 30, 100)]
        [InlineData(10, 10, 17, 0, 70)]
        [InlineData(10, 10, 17, 30, 40)]
        [InlineData(10, 10, 18, 0, 10)]
        [InlineData(10, 10, 18, 10, 10)]
        [InlineData(11, 35, 11, 45, 410)]
        [InlineData(11, 35, 12, 10, 410)]
        [InlineData(11, 35, 12, 45, 410)]
        [InlineData(11, 35, 12, 55, 400)]
        [InlineData(11, 35, 15, 50, 225)]
        [InlineData(11, 35, 16, 0, 215)]
        [InlineData(11, 35, 16, 30, 185)]
        [InlineData(11, 35, 17, 0, 155)]
        [InlineData(11, 35, 17, 30, 125)]
        [InlineData(11, 35, 18, 0, 95)]
        [InlineData(11, 35, 18, 10, 95)]
        [InlineData(11, 45, 12, 10, 420)]
        [InlineData(11, 45, 12, 45, 420)]
        [InlineData(11, 45, 12, 55, 410)]
        [InlineData(11, 45, 15, 50, 235)]
        [InlineData(11, 45, 16, 0, 225)]
        [InlineData(11, 45, 16, 30, 195)]
        [InlineData(11, 45, 17, 0, 165)]
        [InlineData(11, 45, 17, 30, 135)]
        [InlineData(11, 45, 18, 0, 105)]
        [InlineData(11, 45, 18, 10, 105)]
        [InlineData(12, 10, 12, 45, 420)]
        [InlineData(12, 10, 12, 55, 410)]
        [InlineData(12, 10, 15, 50, 235)]
        [InlineData(12, 10, 16, 0, 225)]
        [InlineData(12, 10, 16, 30, 195)]
        [InlineData(12, 10, 17, 0, 165)]
        [InlineData(12, 10, 17, 30, 135)]
        [InlineData(12, 10, 18, 0, 105)]
        [InlineData(12, 10, 18, 10, 105)]
        [InlineData(12, 45, 12, 55, 410)]
        [InlineData(12, 45, 15, 50, 235)]
        [InlineData(12, 45, 16, 0, 225)]
        [InlineData(12, 45, 16, 30, 195)]
        [InlineData(12, 45, 17, 0, 165)]
        [InlineData(12, 45, 17, 30, 135)]
        [InlineData(12, 45, 18, 0, 105)]
        [InlineData(12, 45, 18, 10, 105)]
        [InlineData(12, 55, 15, 50, 245)]
        [InlineData(12, 55, 16, 0, 235)]
        [InlineData(12, 55, 16, 30, 205)]
        [InlineData(12, 55, 17, 0, 175)]
        [InlineData(12, 55, 17, 30, 145)]
        [InlineData(12, 55, 18, 0, 115)]
        [InlineData(12, 55, 18, 10, 115)]
        [InlineData(15, 50, 16, 0, 410)]
        [InlineData(15, 50, 16, 30, 380)]
        [InlineData(15, 50, 17, 0, 350)]
        [InlineData(15, 50, 17, 30, 320)]
        [InlineData(15, 50, 18, 0, 290)]
        [InlineData(15, 50, 18, 10, 290)]
        [InlineData(16, 0, 16, 30, 390)]
        [InlineData(16, 0, 17, 0, 360)]
        [InlineData(16, 0, 17, 30, 330)]
        [InlineData(16, 0, 18, 0, 300)]
        [InlineData(16, 0, 18, 10, 300)]
        [InlineData(16, 30, 17, 0, 390)]
        [InlineData(16, 30, 17, 30, 360)]
        [InlineData(16, 30, 18, 0, 330)]
        [InlineData(16, 30, 18, 10, 330)]
        [InlineData(17, 0, 17, 30, 390)]
        [InlineData(17, 0, 18, 0, 360)]
        [InlineData(17, 0, 18, 10, 360)]
        [InlineData(17, 30, 18, 0, 390)]
        [InlineData(17, 30, 18, 10, 390)]
        [InlineData(18, 0, 18, 10, 420)]
        #endregion
        public void WorkingHoursValidator_DevsQAs_ML_LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime)
        {
            LackingTime(checkInHour, checkInMinute, checkOutHour, checkOutMinute, lackingTime, _devsqas_ml_Group);
        }

        #endregion

        #region Devs / QAs Maternity Leave Early

        #region WorkingHoursRules

        public static WorkingHoursRuleBase _devsqas_me_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
        {
            WorkingTimeStart = new TimeSpan(8, 0, 0),
            WorkingTimeEnd = new TimeSpan(16, 0, 0),
            BreaktimeStart = new TimeSpan(11, 45, 0),
            BreaktimeEnd = new TimeSpan(12, 45, 0)
        };

        public static WorkingHoursRuleBase _devsqas_me_EarlyToleranceWorkingHoursRule = new EarlyToleranceWorkingHoursRule()
        {
            Tolerance = new TimeSpan(1, 0, 0)
        };

        public static WorkingHoursRuleBase _devsqas_me_LateToleranceWorkingHoursRule = new LateToleranceWorkingHoursRule()
        {
            Tolerance = new TimeSpan(1, 0, 0)
        };

        public static WorkingPoliciesGroup _devsqas_me_Group = new WorkingPoliciesGroup()
        {
            WorkingHoursRules = new WorkingHoursRuleBase[]
            {
                _devsqas_me_BaseTimeWorkingHoursRule,
                _devsqas_me_EarlyToleranceWorkingHoursRule,
                _devsqas_me_LateToleranceWorkingHoursRule
            }
        };

        #endregion

        [Theory]
        #region InlineData
        [InlineData(6, 50, 7, 0, 420)]
        [InlineData(6, 50, 7, 30, 390)]
        [InlineData(6, 50, 8, 0, 360)]
        [InlineData(6, 50, 8, 30, 330)]
        [InlineData(6, 50, 9, 0, 300)]
        [InlineData(6, 50, 9, 10, 290)]
        [InlineData(6, 50, 11, 35, 145)]
        [InlineData(6, 50, 11, 45, 135)]
        [InlineData(6, 50, 12, 10, 135)]
        [InlineData(6, 50, 12, 45, 135)]
        [InlineData(6, 50, 12, 55, 125)]
        [InlineData(6, 50, 14, 50, 10)]
        [InlineData(6, 50, 15, 0, 0)]
        [InlineData(6, 50, 15, 30, 0)]
        [InlineData(6, 50, 16, 0, 0)]
        [InlineData(6, 50, 16, 30, 0)]
        [InlineData(6, 50, 17, 0, 0)]
        [InlineData(6, 50, 17, 10, 0)]
        [InlineData(7, 0, 7, 30, 390)]
        [InlineData(7, 0, 8, 0, 360)]
        [InlineData(7, 0, 8, 30, 330)]
        [InlineData(7, 0, 9, 0, 300)]
        [InlineData(7, 0, 9, 10, 290)]
        [InlineData(7, 0, 11, 35, 145)]
        [InlineData(7, 0, 11, 45, 135)]
        [InlineData(7, 0, 12, 10, 135)]
        [InlineData(7, 0, 12, 45, 135)]
        [InlineData(7, 0, 12, 55, 125)]
        [InlineData(7, 0, 14, 50, 10)]
        [InlineData(7, 0, 15, 0, 0)]
        [InlineData(7, 0, 15, 30, 0)]
        [InlineData(7, 0, 16, 0, 0)]
        [InlineData(7, 0, 16, 30, 0)]
        [InlineData(7, 0, 17, 0, 0)]
        [InlineData(7, 0, 17, 10, 0)]
        [InlineData(7, 30, 8, 0, 390)]
        [InlineData(7, 30, 8, 30, 360)]
        [InlineData(7, 30, 9, 0, 330)]
        [InlineData(7, 30, 9, 10, 320)]
        [InlineData(7, 30, 11, 35, 175)]
        [InlineData(7, 30, 11, 45, 165)]
        [InlineData(7, 30, 12, 10, 165)]
        [InlineData(7, 30, 12, 45, 165)]
        [InlineData(7, 30, 12, 55, 155)]
        [InlineData(7, 30, 14, 50, 40)]
        [InlineData(7, 30, 15, 0, 30)]
        [InlineData(7, 30, 15, 30, 0)]
        [InlineData(7, 30, 16, 0, 0)]
        [InlineData(7, 30, 16, 30, 0)]
        [InlineData(7, 30, 17, 0, 0)]
        [InlineData(7, 30, 17, 10, 0)]
        [InlineData(8, 0, 8, 30, 390)]
        [InlineData(8, 0, 9, 0, 360)]
        [InlineData(8, 0, 9, 10, 350)]
        [InlineData(8, 0, 11, 35, 205)]
        [InlineData(8, 0, 11, 45, 195)]
        [InlineData(8, 0, 12, 10, 195)]
        [InlineData(8, 0, 12, 45, 195)]
        [InlineData(8, 0, 12, 55, 185)]
        [InlineData(8, 0, 14, 50, 70)]
        [InlineData(8, 0, 15, 0, 60)]
        [InlineData(8, 0, 15, 30, 30)]
        [InlineData(8, 0, 16, 0, 0)]
        [InlineData(8, 0, 16, 30, 0)]
        [InlineData(8, 0, 17, 0, 0)]
        [InlineData(8, 0, 17, 10, 0)]
        [InlineData(8, 30, 9, 0, 390)]
        [InlineData(8, 30, 9, 10, 380)]
        [InlineData(8, 30, 11, 35, 235)]
        [InlineData(8, 30, 11, 45, 225)]
        [InlineData(8, 30, 12, 10, 225)]
        [InlineData(8, 30, 12, 45, 225)]
        [InlineData(8, 30, 12, 55, 215)]
        [InlineData(8, 30, 14, 50, 100)]
        [InlineData(8, 30, 15, 0, 90)]
        [InlineData(8, 30, 15, 30, 60)]
        [InlineData(8, 30, 16, 0, 30)]
        [InlineData(8, 30, 16, 30, 0)]
        [InlineData(8, 30, 17, 0, 0)]
        [InlineData(8, 30, 17, 10, 0)]
        [InlineData(9, 0, 9, 10, 410)]
        [InlineData(9, 0, 11, 35, 265)]
        [InlineData(9, 0, 11, 45, 255)]
        [InlineData(9, 0, 12, 10, 255)]
        [InlineData(9, 0, 12, 45, 255)]
        [InlineData(9, 0, 12, 55, 245)]
        [InlineData(9, 0, 14, 50, 130)]
        [InlineData(9, 0, 15, 0, 120)]
        [InlineData(9, 0, 15, 30, 90)]
        [InlineData(9, 0, 16, 0, 60)]
        [InlineData(9, 0, 16, 30, 30)]
        [InlineData(9, 0, 17, 0, 0)]
        [InlineData(9, 0, 17, 10, 0)]
        [InlineData(9, 10, 11, 35, 275)]
        [InlineData(9, 10, 11, 45, 265)]
        [InlineData(9, 10, 12, 10, 265)]
        [InlineData(9, 10, 12, 45, 265)]
        [InlineData(9, 10, 12, 55, 255)]
        [InlineData(9, 10, 14, 50, 140)]
        [InlineData(9, 10, 15, 0, 130)]
        [InlineData(9, 10, 15, 30, 100)]
        [InlineData(9, 10, 16, 0, 70)]
        [InlineData(9, 10, 16, 30, 40)]
        [InlineData(9, 10, 17, 0, 10)]
        [InlineData(9, 10, 17, 10, 10)]
        [InlineData(11, 35, 11, 45, 410)]
        [InlineData(11, 35, 12, 10, 410)]
        [InlineData(11, 35, 12, 45, 410)]
        [InlineData(11, 35, 12, 55, 400)]
        [InlineData(11, 35, 14, 50, 285)]
        [InlineData(11, 35, 15, 0, 275)]
        [InlineData(11, 35, 15, 30, 245)]
        [InlineData(11, 35, 16, 0, 215)]
        [InlineData(11, 35, 16, 30, 185)]
        [InlineData(11, 35, 17, 0, 155)]
        [InlineData(11, 35, 17, 10, 155)]
        [InlineData(11, 45, 12, 10, 420)]
        [InlineData(11, 45, 12, 45, 420)]
        [InlineData(11, 45, 12, 55, 410)]
        [InlineData(11, 45, 14, 50, 295)]
        [InlineData(11, 45, 15, 0, 285)]
        [InlineData(11, 45, 15, 30, 255)]
        [InlineData(11, 45, 16, 0, 225)]
        [InlineData(11, 45, 16, 30, 195)]
        [InlineData(11, 45, 17, 0, 165)]
        [InlineData(11, 45, 17, 10, 165)]
        [InlineData(12, 10, 12, 45, 420)]
        [InlineData(12, 10, 12, 55, 410)]
        [InlineData(12, 10, 14, 50, 295)]
        [InlineData(12, 10, 15, 0, 285)]
        [InlineData(12, 10, 15, 30, 255)]
        [InlineData(12, 10, 16, 0, 225)]
        [InlineData(12, 10, 16, 30, 195)]
        [InlineData(12, 10, 17, 0, 165)]
        [InlineData(12, 10, 17, 10, 165)]
        [InlineData(12, 45, 12, 55, 410)]
        [InlineData(12, 45, 14, 50, 295)]
        [InlineData(12, 45, 15, 0, 285)]
        [InlineData(12, 45, 15, 30, 255)]
        [InlineData(12, 45, 16, 0, 225)]
        [InlineData(12, 45, 16, 30, 195)]
        [InlineData(12, 45, 17, 0, 165)]
        [InlineData(12, 45, 17, 10, 165)]
        [InlineData(12, 55, 14, 50, 305)]
        [InlineData(12, 55, 15, 0, 295)]
        [InlineData(12, 55, 15, 30, 265)]
        [InlineData(12, 55, 16, 0, 235)]
        [InlineData(12, 55, 16, 30, 205)]
        [InlineData(12, 55, 17, 0, 175)]
        [InlineData(12, 55, 17, 10, 175)]
        [InlineData(14, 50, 15, 0, 410)]
        [InlineData(14, 50, 15, 30, 380)]
        [InlineData(14, 50, 16, 0, 350)]
        [InlineData(14, 50, 16, 30, 320)]
        [InlineData(14, 50, 17, 0, 290)]
        [InlineData(14, 50, 17, 10, 290)]
        [InlineData(15, 0, 15, 30, 390)]
        [InlineData(15, 0, 16, 0, 360)]
        [InlineData(15, 0, 16, 30, 330)]
        [InlineData(15, 0, 17, 0, 300)]
        [InlineData(15, 0, 17, 10, 300)]
        [InlineData(15, 30, 16, 0, 390)]
        [InlineData(15, 30, 16, 30, 360)]
        [InlineData(15, 30, 17, 0, 330)]
        [InlineData(15, 30, 17, 10, 330)]
        [InlineData(16, 0, 16, 30, 390)]
        [InlineData(16, 0, 17, 0, 360)]
        [InlineData(16, 0, 17, 10, 360)]
        [InlineData(16, 30, 17, 0, 390)]
        [InlineData(16, 30, 17, 10, 390)]
        [InlineData(17, 0, 17, 10, 420)]
        #endregion
        public void WorkingHoursValidator_DevsQAs_ME_LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime)
        {
            LackingTime(checkInHour, checkInMinute, checkOutHour, checkOutMinute, lackingTime, _devsqas_me_Group);
        }

        #endregion

        #region Special 1

        #region WorkingHoursRules

        public static WorkingHoursRuleBase _special1_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
        {
            WorkingTimeStart = new TimeSpan(8, 30, 0),
            WorkingTimeEnd = new TimeSpan(17, 30, 0),
            BreaktimeStart = new TimeSpan(12, 0, 0),
            BreaktimeEnd = new TimeSpan(13, 0, 0)
        };

        public static WorkingPoliciesGroup _special1_Group = new WorkingPoliciesGroup()
        {
            WorkingHoursRules = new WorkingHoursRuleBase[]
            {
                _special1_BaseTimeWorkingHoursRule
            }
        };

        #endregion

        [Theory]
        #region InlineData
        [InlineData(8, 20, 8, 30, 480)]
        [InlineData(8, 20, 8, 40, 470)]
        [InlineData(8, 20, 11, 50, 280)]
        [InlineData(8, 20, 12, 0, 270)]
        [InlineData(8, 20, 12, 30, 270)]
        [InlineData(8, 20, 13, 0, 270)]
        [InlineData(8, 20, 13, 10, 260)]
        [InlineData(8, 20, 17, 20, 10)]
        [InlineData(8, 20, 17, 30, 0)]
        [InlineData(8, 20, 17, 40, 0)]
        [InlineData(8, 30, 8, 40, 470)]
        [InlineData(8, 30, 11, 50, 280)]
        [InlineData(8, 30, 12, 0, 270)]
        [InlineData(8, 30, 12, 30, 270)]
        [InlineData(8, 30, 13, 0, 270)]
        [InlineData(8, 30, 13, 10, 260)]
        [InlineData(8, 30, 17, 20, 10)]
        [InlineData(8, 30, 17, 30, 0)]
        [InlineData(8, 30, 17, 40, 0)]
        [InlineData(8, 40, 11, 50, 290)]
        [InlineData(8, 40, 12, 0, 280)]
        [InlineData(8, 40, 12, 30, 280)]
        [InlineData(8, 40, 13, 0, 280)]
        [InlineData(8, 40, 13, 10, 270)]
        [InlineData(8, 40, 17, 20, 20)]
        [InlineData(8, 40, 17, 30, 10)]
        [InlineData(8, 40, 17, 40, 10)]
        [InlineData(11, 50, 12, 0, 470)]
        [InlineData(11, 50, 12, 30, 470)]
        [InlineData(11, 50, 13, 0, 470)]
        [InlineData(11, 50, 13, 10, 460)]
        [InlineData(11, 50, 17, 20, 210)]
        [InlineData(11, 50, 17, 30, 200)]
        [InlineData(11, 50, 17, 40, 200)]
        [InlineData(12, 0, 12, 30, 480)]
        [InlineData(12, 0, 13, 0, 480)]
        [InlineData(12, 0, 13, 10, 470)]
        [InlineData(12, 0, 17, 20, 220)]
        [InlineData(12, 0, 17, 30, 210)]
        [InlineData(12, 0, 17, 40, 210)]
        [InlineData(12, 30, 13, 0, 480)]
        [InlineData(12, 30, 13, 10, 470)]
        [InlineData(12, 30, 17, 20, 220)]
        [InlineData(12, 30, 17, 30, 210)]
        [InlineData(12, 30, 17, 40, 210)]
        [InlineData(13, 0, 13, 10, 470)]
        [InlineData(13, 0, 17, 20, 220)]
        [InlineData(13, 0, 17, 30, 210)]
        [InlineData(13, 0, 17, 40, 210)]
        [InlineData(13, 10, 17, 20, 230)]
        [InlineData(13, 10, 17, 30, 220)]
        [InlineData(13, 10, 17, 40, 220)]
        [InlineData(17, 20, 17, 30, 470)]
        [InlineData(17, 20, 17, 40, 470)]
        [InlineData(17, 30, 17, 40, 480)]
        #endregion
        public void WorkingHoursValidator_Special1_LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime)
        {
            LackingTime(checkInHour, checkInMinute, checkOutHour, checkOutMinute, lackingTime, _special1_Group);
        }

        #endregion

        #region Special 1 Maternity Arrive Late

        #region WorkingHoursRules

        public static WorkingHoursRuleBase _special1_ml_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
        {
            WorkingTimeStart = new TimeSpan(9, 30, 0),
            WorkingTimeEnd = new TimeSpan(17, 30, 0),
            BreaktimeStart = new TimeSpan(12, 0, 0),
            BreaktimeEnd = new TimeSpan(13, 0, 0)
        };

        public static WorkingPoliciesGroup _special1_ml_Group = new WorkingPoliciesGroup()
        {
            WorkingHoursRules = new WorkingHoursRuleBase[]
            {
                _special1_ml_BaseTimeWorkingHoursRule
            }
        };

        #endregion

        [Theory]
        #region InlineData
        [InlineData(9, 20, 9, 30, 420)]
        [InlineData(9, 20, 9, 40, 410)]
        [InlineData(9, 20, 11, 50, 280)]
        [InlineData(9, 20, 12, 0, 270)]
        [InlineData(9, 20, 12, 30, 270)]
        [InlineData(9, 20, 13, 0, 270)]
        [InlineData(9, 20, 13, 10, 260)]
        [InlineData(9, 20, 17, 20, 10)]
        [InlineData(9, 20, 17, 30, 0)]
        [InlineData(9, 20, 17, 40, 0)]
        [InlineData(9, 30, 9, 40, 410)]
        [InlineData(9, 30, 11, 50, 280)]
        [InlineData(9, 30, 12, 0, 270)]
        [InlineData(9, 30, 12, 30, 270)]
        [InlineData(9, 30, 13, 0, 270)]
        [InlineData(9, 30, 13, 10, 260)]
        [InlineData(9, 30, 17, 20, 10)]
        [InlineData(9, 30, 17, 30, 0)]
        [InlineData(9, 30, 17, 40, 0)]
        [InlineData(9, 40, 11, 50, 290)]
        [InlineData(9, 40, 12, 0, 280)]
        [InlineData(9, 40, 12, 30, 280)]
        [InlineData(9, 40, 13, 0, 280)]
        [InlineData(9, 40, 13, 10, 270)]
        [InlineData(9, 40, 17, 20, 20)]
        [InlineData(9, 40, 17, 30, 10)]
        [InlineData(9, 40, 17, 40, 10)]
        [InlineData(11, 50, 12, 0, 410)]
        [InlineData(11, 50, 12, 30, 410)]
        [InlineData(11, 50, 13, 0, 410)]
        [InlineData(11, 50, 13, 10, 400)]
        [InlineData(11, 50, 17, 20, 150)]
        [InlineData(11, 50, 17, 30, 140)]
        [InlineData(11, 50, 17, 40, 140)]
        [InlineData(12, 0, 12, 30, 420)]
        [InlineData(12, 0, 13, 0, 420)]
        [InlineData(12, 0, 13, 10, 410)]
        [InlineData(12, 0, 17, 20, 160)]
        [InlineData(12, 0, 17, 30, 150)]
        [InlineData(12, 0, 17, 40, 150)]
        [InlineData(12, 30, 13, 0, 420)]
        [InlineData(12, 30, 13, 10, 410)]
        [InlineData(12, 30, 17, 20, 160)]
        [InlineData(12, 30, 17, 30, 150)]
        [InlineData(12, 30, 17, 40, 150)]
        [InlineData(13, 0, 13, 10, 410)]
        [InlineData(13, 0, 17, 20, 160)]
        [InlineData(13, 0, 17, 30, 150)]
        [InlineData(13, 0, 17, 40, 150)]
        [InlineData(13, 10, 17, 20, 170)]
        [InlineData(13, 10, 17, 30, 160)]
        [InlineData(13, 10, 17, 40, 160)]
        [InlineData(17, 20, 17, 30, 410)]
        [InlineData(17, 20, 17, 40, 410)]
        [InlineData(17, 30, 17, 40, 420)]
        #endregion
        public void WorkingHoursValidator_Special1_ML_LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime)
        {
            LackingTime(checkInHour, checkInMinute, checkOutHour, checkOutMinute, lackingTime, _special1_ml_Group);
        }

        #endregion

        #region Special 1 Maternity Arrive Late

        #region WorkingHoursRules

        public static WorkingHoursRuleBase _special1_me_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
        {
            WorkingTimeStart = new TimeSpan(8, 30, 0),
            WorkingTimeEnd = new TimeSpan(16, 30, 0),
            BreaktimeStart = new TimeSpan(12, 0, 0),
            BreaktimeEnd = new TimeSpan(13, 0, 0)
        };

        public static WorkingPoliciesGroup _special1_me_Group = new WorkingPoliciesGroup()
        {
            WorkingHoursRules = new WorkingHoursRuleBase[]
            {
                _special1_me_BaseTimeWorkingHoursRule
            }
        };

        #endregion

        [Theory]
        #region InlineData
        [InlineData(8, 20, 8, 30, 420)]
        [InlineData(8, 20, 8, 40, 410)]
        [InlineData(8, 20, 11, 50, 220)]
        [InlineData(8, 20, 12, 0, 210)]
        [InlineData(8, 20, 12, 30, 210)]
        [InlineData(8, 20, 13, 0, 210)]
        [InlineData(8, 20, 13, 10, 200)]
        [InlineData(8, 20, 16, 20, 10)]
        [InlineData(8, 20, 16, 30, 0)]
        [InlineData(8, 20, 16, 40, 0)]
        [InlineData(8, 30, 8, 40, 410)]
        [InlineData(8, 30, 11, 50, 220)]
        [InlineData(8, 30, 12, 0, 210)]
        [InlineData(8, 30, 12, 30, 210)]
        [InlineData(8, 30, 13, 0, 210)]
        [InlineData(8, 30, 13, 10, 200)]
        [InlineData(8, 30, 16, 20, 10)]
        [InlineData(8, 30, 16, 30, 0)]
        [InlineData(8, 30, 16, 40, 0)]
        [InlineData(8, 40, 11, 50, 230)]
        [InlineData(8, 40, 12, 0, 220)]
        [InlineData(8, 40, 12, 30, 220)]
        [InlineData(8, 40, 13, 0, 220)]
        [InlineData(8, 40, 13, 10, 210)]
        [InlineData(8, 40, 16, 20, 20)]
        [InlineData(8, 40, 16, 30, 10)]
        [InlineData(8, 40, 16, 40, 10)]
        [InlineData(11, 50, 12, 0, 410)]
        [InlineData(11, 50, 12, 30, 410)]
        [InlineData(11, 50, 13, 0, 410)]
        [InlineData(11, 50, 13, 10, 400)]
        [InlineData(11, 50, 16, 20, 210)]
        [InlineData(11, 50, 16, 30, 200)]
        [InlineData(11, 50, 16, 40, 200)]
        [InlineData(12, 0, 12, 30, 420)]
        [InlineData(12, 0, 13, 0, 420)]
        [InlineData(12, 0, 13, 10, 410)]
        [InlineData(12, 0, 16, 20, 220)]
        [InlineData(12, 0, 16, 30, 210)]
        [InlineData(12, 0, 16, 40, 210)]
        [InlineData(12, 30, 13, 0, 420)]
        [InlineData(12, 30, 13, 10, 410)]
        [InlineData(12, 30, 16, 20, 220)]
        [InlineData(12, 30, 16, 30, 210)]
        [InlineData(12, 30, 16, 40, 210)]
        [InlineData(13, 0, 13, 10, 410)]
        [InlineData(13, 0, 16, 20, 220)]
        [InlineData(13, 0, 16, 30, 210)]
        [InlineData(13, 0, 16, 40, 210)]
        [InlineData(13, 10, 16, 20, 230)]
        [InlineData(13, 10, 16, 30, 220)]
        [InlineData(13, 10, 16, 40, 220)]
        [InlineData(16, 20, 16, 30, 410)]
        [InlineData(16, 20, 16, 40, 410)]
        [InlineData(16, 30, 16, 40, 420)]
        #endregion
        public void WorkingHoursValidator_Special1_ME_LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime)
        {
            LackingTime(checkInHour, checkInMinute, checkOutHour, checkOutMinute, lackingTime, _special1_me_Group);
        }

        #endregion

        #region Special 2

        #region WorkingHoursRules

        public static WorkingHoursRuleBase _special2_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
        {
            WorkingTimeStart = new TimeSpan(8, 0, 0),
            WorkingTimeEnd = new TimeSpan(17, 0, 0),
            BreaktimeStart = new TimeSpan(12, 0, 0),
            BreaktimeEnd = new TimeSpan(13, 0, 0)
        };

        public static WorkingHoursRuleBase _special2_LateToleranceWorkingHoursRule = new LateToleranceWorkingHoursRule()
        {
            Tolerance = new TimeSpan(0, 30, 0)
        };

        public static WorkingPoliciesGroup _special2_Group = new WorkingPoliciesGroup()
        {
            WorkingHoursRules = new WorkingHoursRuleBase[]
            {
                _special2_BaseTimeWorkingHoursRule,
                _special2_LateToleranceWorkingHoursRule
            }
        };

        #endregion

        [Theory]
        #region InlineData
        [InlineData(7, 50, 8, 0, 480)]
        [InlineData(7, 50, 8, 15, 465)]
        [InlineData(7, 50, 8, 30, 450)]
        [InlineData(7, 50, 8, 40, 440)]
        [InlineData(7, 50, 11, 50, 250)]
        [InlineData(7, 50, 12, 0, 240)]
        [InlineData(7, 50, 12, 30, 240)]
        [InlineData(7, 50, 13, 0, 240)]
        [InlineData(7, 50, 13, 10, 230)]
        [InlineData(7, 50, 16, 50, 10)]
        [InlineData(7, 50, 17, 0, 0)]
        [InlineData(7, 50, 17, 15, 0)]
        [InlineData(7, 50, 17, 30, 0)]
        [InlineData(7, 50, 17, 40, 0)]
        [InlineData(8, 0, 8, 15, 465)]
        [InlineData(8, 0, 8, 30, 450)]
        [InlineData(8, 0, 8, 40, 440)]
        [InlineData(8, 0, 11, 50, 250)]
        [InlineData(8, 0, 12, 0, 240)]
        [InlineData(8, 0, 12, 30, 240)]
        [InlineData(8, 0, 13, 0, 240)]
        [InlineData(8, 0, 13, 10, 230)]
        [InlineData(8, 0, 16, 50, 10)]
        [InlineData(8, 0, 17, 0, 0)]
        [InlineData(8, 0, 17, 15, 0)]
        [InlineData(8, 0, 17, 30, 0)]
        [InlineData(8, 0, 17, 40, 0)]
        [InlineData(8, 15, 8, 30, 465)]
        [InlineData(8, 15, 8, 40, 455)]
        [InlineData(8, 15, 11, 50, 265)]
        [InlineData(8, 15, 12, 0, 255)]
        [InlineData(8, 15, 12, 30, 255)]
        [InlineData(8, 15, 13, 0, 255)]
        [InlineData(8, 15, 13, 10, 245)]
        [InlineData(8, 15, 16, 50, 25)]
        [InlineData(8, 15, 17, 0, 15)]
        [InlineData(8, 15, 17, 15, 0)]
        [InlineData(8, 15, 17, 30, 0)]
        [InlineData(8, 15, 17, 40, 0)]
        [InlineData(8, 30, 8, 40, 470)]
        [InlineData(8, 30, 11, 50, 280)]
        [InlineData(8, 30, 12, 0, 270)]
        [InlineData(8, 30, 12, 30, 270)]
        [InlineData(8, 30, 13, 0, 270)]
        [InlineData(8, 30, 13, 10, 260)]
        [InlineData(8, 30, 16, 50, 40)]
        [InlineData(8, 30, 17, 0, 30)]
        [InlineData(8, 30, 17, 15, 15)]
        [InlineData(8, 30, 17, 30, 0)]
        [InlineData(8, 30, 17, 40, 0)]
        [InlineData(8, 40, 11, 50, 290)]
        [InlineData(8, 40, 12, 0, 280)]
        [InlineData(8, 40, 12, 30, 280)]
        [InlineData(8, 40, 13, 0, 280)]
        [InlineData(8, 40, 13, 10, 270)]
        [InlineData(8, 40, 16, 50, 50)]
        [InlineData(8, 40, 17, 0, 40)]
        [InlineData(8, 40, 17, 15, 25)]
        [InlineData(8, 40, 17, 30, 10)]
        [InlineData(8, 40, 17, 40, 10)]
        [InlineData(11, 50, 12, 0, 470)]
        [InlineData(11, 50, 12, 30, 470)]
        [InlineData(11, 50, 13, 0, 470)]
        [InlineData(11, 50, 13, 10, 460)]
        [InlineData(11, 50, 16, 50, 240)]
        [InlineData(11, 50, 17, 0, 230)]
        [InlineData(11, 50, 17, 15, 215)]
        [InlineData(11, 50, 17, 30, 200)]
        [InlineData(11, 50, 17, 40, 200)]
        [InlineData(12, 0, 12, 30, 480)]
        [InlineData(12, 0, 13, 0, 480)]
        [InlineData(12, 0, 13, 10, 470)]
        [InlineData(12, 0, 16, 50, 250)]
        [InlineData(12, 0, 17, 0, 240)]
        [InlineData(12, 0, 17, 15, 225)]
        [InlineData(12, 0, 17, 30, 210)]
        [InlineData(12, 0, 17, 40, 210)]
        [InlineData(12, 30, 13, 0, 480)]
        [InlineData(12, 30, 13, 10, 470)]
        [InlineData(12, 30, 16, 50, 250)]
        [InlineData(12, 30, 17, 0, 240)]
        [InlineData(12, 30, 17, 15, 225)]
        [InlineData(12, 30, 17, 30, 210)]
        [InlineData(12, 30, 17, 40, 210)]
        [InlineData(13, 0, 13, 10, 470)]
        [InlineData(13, 0, 16, 50, 250)]
        [InlineData(13, 0, 17, 0, 240)]
        [InlineData(13, 0, 17, 15, 225)]
        [InlineData(13, 0, 17, 30, 210)]
        [InlineData(13, 0, 17, 40, 210)]
        [InlineData(13, 10, 16, 50, 260)]
        [InlineData(13, 10, 17, 0, 250)]
        [InlineData(13, 10, 17, 15, 235)]
        [InlineData(13, 10, 17, 30, 220)]
        [InlineData(13, 10, 17, 40, 220)]
        [InlineData(16, 50, 17, 0, 470)]
        [InlineData(16, 50, 17, 15, 455)]
        [InlineData(16, 50, 17, 30, 440)]
        [InlineData(16, 50, 17, 40, 440)]
        [InlineData(17, 0, 17, 15, 465)]
        [InlineData(17, 0, 17, 30, 450)]
        [InlineData(17, 0, 17, 40, 450)]
        [InlineData(17, 15, 17, 30, 465)]
        [InlineData(17, 15, 17, 40, 465)]
        [InlineData(17, 30, 17, 40, 480)]
        #endregion
        public void WorkingHoursValidator_Special2_LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime)
        {
            LackingTime(checkInHour, checkInMinute, checkOutHour, checkOutMinute, lackingTime, _special2_Group);
        }

        #endregion

        #region Special 2 Maternity Arrive Late

        #region WorkingHoursRules

        public static WorkingHoursRuleBase _special2_ml_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
        {
            WorkingTimeStart = new TimeSpan(9, 0, 0),
            WorkingTimeEnd = new TimeSpan(17, 0, 0),
            BreaktimeStart = new TimeSpan(12, 0, 0),
            BreaktimeEnd = new TimeSpan(13, 0, 0)
        };

        public static WorkingHoursRuleBase _special2_ml_LateToleranceWorkingHoursRule = new LateToleranceWorkingHoursRule()
        {
            Tolerance = new TimeSpan(0, 30, 0)
        };

        public static WorkingPoliciesGroup _special2_ml_Group = new WorkingPoliciesGroup()
        {
            WorkingHoursRules = new WorkingHoursRuleBase[]
            {
                _special2_ml_BaseTimeWorkingHoursRule,
                _special2_ml_LateToleranceWorkingHoursRule
            }
        };

        #endregion

        [Theory]
        #region InlineData
        [InlineData(8, 50, 9, 0, 420)]
        [InlineData(8, 50, 9, 15, 405)]
        [InlineData(8, 50, 9, 30, 390)]
        [InlineData(8, 50, 9, 40, 380)]
        [InlineData(8, 50, 11, 50, 250)]
        [InlineData(8, 50, 12, 0, 240)]
        [InlineData(8, 50, 12, 30, 240)]
        [InlineData(8, 50, 13, 0, 240)]
        [InlineData(8, 50, 13, 10, 230)]
        [InlineData(8, 50, 16, 50, 10)]
        [InlineData(8, 50, 17, 0, 0)]
        [InlineData(8, 50, 17, 15, 0)]
        [InlineData(8, 50, 17, 30, 0)]
        [InlineData(8, 50, 17, 40, 0)]
        [InlineData(9, 0, 9, 15, 405)]
        [InlineData(9, 0, 9, 30, 390)]
        [InlineData(9, 0, 9, 40, 380)]
        [InlineData(9, 0, 11, 50, 250)]
        [InlineData(9, 0, 12, 0, 240)]
        [InlineData(9, 0, 12, 30, 240)]
        [InlineData(9, 0, 13, 0, 240)]
        [InlineData(9, 0, 13, 10, 230)]
        [InlineData(9, 0, 16, 50, 10)]
        [InlineData(9, 0, 17, 0, 0)]
        [InlineData(9, 0, 17, 15, 0)]
        [InlineData(9, 0, 17, 30, 0)]
        [InlineData(9, 0, 17, 40, 0)]
        [InlineData(9, 15, 9, 30, 405)]
        [InlineData(9, 15, 9, 40, 395)]
        [InlineData(9, 15, 11, 50, 265)]
        [InlineData(9, 15, 12, 0, 255)]
        [InlineData(9, 15, 12, 30, 255)]
        [InlineData(9, 15, 13, 0, 255)]
        [InlineData(9, 15, 13, 10, 245)]
        [InlineData(9, 15, 16, 50, 25)]
        [InlineData(9, 15, 17, 0, 15)]
        [InlineData(9, 15, 17, 15, 0)]
        [InlineData(9, 15, 17, 30, 0)]
        [InlineData(9, 15, 17, 40, 0)]
        [InlineData(9, 30, 9, 40, 410)]
        [InlineData(9, 30, 11, 50, 280)]
        [InlineData(9, 30, 12, 0, 270)]
        [InlineData(9, 30, 12, 30, 270)]
        [InlineData(9, 30, 13, 0, 270)]
        [InlineData(9, 30, 13, 10, 260)]
        [InlineData(9, 30, 16, 50, 40)]
        [InlineData(9, 30, 17, 0, 30)]
        [InlineData(9, 30, 17, 15, 15)]
        [InlineData(9, 30, 17, 30, 0)]
        [InlineData(9, 30, 17, 40, 0)]
        [InlineData(9, 40, 11, 50, 290)]
        [InlineData(9, 40, 12, 0, 280)]
        [InlineData(9, 40, 12, 30, 280)]
        [InlineData(9, 40, 13, 0, 280)]
        [InlineData(9, 40, 13, 10, 270)]
        [InlineData(9, 40, 16, 50, 50)]
        [InlineData(9, 40, 17, 0, 40)]
        [InlineData(9, 40, 17, 15, 25)]
        [InlineData(9, 40, 17, 30, 10)]
        [InlineData(9, 40, 17, 40, 10)]
        [InlineData(11, 50, 12, 0, 410)]
        [InlineData(11, 50, 12, 30, 410)]
        [InlineData(11, 50, 13, 0, 410)]
        [InlineData(11, 50, 13, 10, 400)]
        [InlineData(11, 50, 16, 50, 180)]
        [InlineData(11, 50, 17, 0, 170)]
        [InlineData(11, 50, 17, 15, 155)]
        [InlineData(11, 50, 17, 30, 140)]
        [InlineData(11, 50, 17, 40, 140)]
        [InlineData(12, 0, 12, 30, 420)]
        [InlineData(12, 0, 13, 0, 420)]
        [InlineData(12, 0, 13, 10, 410)]
        [InlineData(12, 0, 16, 50, 190)]
        [InlineData(12, 0, 17, 0, 180)]
        [InlineData(12, 0, 17, 15, 165)]
        [InlineData(12, 0, 17, 30, 150)]
        [InlineData(12, 0, 17, 40, 150)]
        [InlineData(12, 30, 13, 0, 420)]
        [InlineData(12, 30, 13, 10, 410)]
        [InlineData(12, 30, 16, 50, 190)]
        [InlineData(12, 30, 17, 0, 180)]
        [InlineData(12, 30, 17, 15, 165)]
        [InlineData(12, 30, 17, 30, 150)]
        [InlineData(12, 30, 17, 40, 150)]
        [InlineData(13, 0, 13, 10, 410)]
        [InlineData(13, 0, 16, 50, 190)]
        [InlineData(13, 0, 17, 0, 180)]
        [InlineData(13, 0, 17, 15, 165)]
        [InlineData(13, 0, 17, 30, 150)]
        [InlineData(13, 0, 17, 40, 150)]
        [InlineData(13, 10, 16, 50, 200)]
        [InlineData(13, 10, 17, 0, 190)]
        [InlineData(13, 10, 17, 15, 175)]
        [InlineData(13, 10, 17, 30, 160)]
        [InlineData(13, 10, 17, 40, 160)]
        [InlineData(16, 50, 17, 0, 410)]
        [InlineData(16, 50, 17, 15, 395)]
        [InlineData(16, 50, 17, 30, 380)]
        [InlineData(16, 50, 17, 40, 380)]
        [InlineData(17, 0, 17, 15, 405)]
        [InlineData(17, 0, 17, 30, 390)]
        [InlineData(17, 0, 17, 40, 390)]
        [InlineData(17, 15, 17, 30, 405)]
        [InlineData(17, 15, 17, 40, 405)]
        [InlineData(17, 30, 17, 40, 420)]
        #endregion
        public void WorkingHoursValidator_Special2_ML_LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime)
        {
            LackingTime(checkInHour, checkInMinute, checkOutHour, checkOutMinute, lackingTime, _special2_ml_Group);
        }

        #endregion

        #region Special 2 Maternity Leave Early

        #region WorkingHoursRules

        public static WorkingHoursRuleBase _special2_me_BaseTimeWorkingHoursRule = new BaseTimeWorkingHoursRule()
        {
            WorkingTimeStart = new TimeSpan(8, 0, 0),
            WorkingTimeEnd = new TimeSpan(16, 0, 0),
            BreaktimeStart = new TimeSpan(12, 0, 0),
            BreaktimeEnd = new TimeSpan(13, 0, 0)
        };

        public static WorkingHoursRuleBase _special2_me_LateToleranceWorkingHoursRule = new LateToleranceWorkingHoursRule()
        {
            Tolerance = new TimeSpan(0, 30, 0)
        };

        public static WorkingPoliciesGroup _special2_me_Group = new WorkingPoliciesGroup()
        {
            WorkingHoursRules = new WorkingHoursRuleBase[]
            {
                _special2_me_BaseTimeWorkingHoursRule,
                _special2_me_LateToleranceWorkingHoursRule
            }
        };

        #endregion

        [Theory]
        #region InlineData
        [InlineData(7, 50, 8, 0, 420)]
        [InlineData(7, 50, 8, 15, 405)]
        [InlineData(7, 50, 8, 30, 390)]
        [InlineData(7, 50, 8, 40, 380)]
        [InlineData(7, 50, 11, 50, 190)]
        [InlineData(7, 50, 12, 0, 180)]
        [InlineData(7, 50, 12, 30, 180)]
        [InlineData(7, 50, 13, 0, 180)]
        [InlineData(7, 50, 13, 10, 170)]
        [InlineData(7, 50, 15, 50, 10)]
        [InlineData(7, 50, 16, 0, 0)]
        [InlineData(7, 50, 16, 15, 0)]
        [InlineData(7, 50, 16, 30, 0)]
        [InlineData(7, 50, 16, 40, 0)]
        [InlineData(8, 0, 8, 15, 405)]
        [InlineData(8, 0, 8, 30, 390)]
        [InlineData(8, 0, 8, 40, 380)]
        [InlineData(8, 0, 11, 50, 190)]
        [InlineData(8, 0, 12, 0, 180)]
        [InlineData(8, 0, 12, 30, 180)]
        [InlineData(8, 0, 13, 0, 180)]
        [InlineData(8, 0, 13, 10, 170)]
        [InlineData(8, 0, 15, 50, 10)]
        [InlineData(8, 0, 16, 0, 0)]
        [InlineData(8, 0, 16, 15, 0)]
        [InlineData(8, 0, 16, 30, 0)]
        [InlineData(8, 0, 16, 40, 0)]
        [InlineData(8, 15, 8, 30, 405)]
        [InlineData(8, 15, 8, 40, 395)]
        [InlineData(8, 15, 11, 50, 205)]
        [InlineData(8, 15, 12, 0, 195)]
        [InlineData(8, 15, 12, 30, 195)]
        [InlineData(8, 15, 13, 0, 195)]
        [InlineData(8, 15, 13, 10, 185)]
        [InlineData(8, 15, 15, 50, 25)]
        [InlineData(8, 15, 16, 0, 15)]
        [InlineData(8, 15, 16, 15, 0)]
        [InlineData(8, 15, 16, 30, 0)]
        [InlineData(8, 15, 16, 40, 0)]
        [InlineData(8, 30, 8, 40, 410)]
        [InlineData(8, 30, 11, 50, 220)]
        [InlineData(8, 30, 12, 0, 210)]
        [InlineData(8, 30, 12, 30, 210)]
        [InlineData(8, 30, 13, 0, 210)]
        [InlineData(8, 30, 13, 10, 200)]
        [InlineData(8, 30, 15, 50, 40)]
        [InlineData(8, 30, 16, 0, 30)]
        [InlineData(8, 30, 16, 15, 15)]
        [InlineData(8, 30, 16, 30, 0)]
        [InlineData(8, 30, 16, 40, 0)]
        [InlineData(8, 40, 11, 50, 230)]
        [InlineData(8, 40, 12, 0, 220)]
        [InlineData(8, 40, 12, 30, 220)]
        [InlineData(8, 40, 13, 0, 220)]
        [InlineData(8, 40, 13, 10, 210)]
        [InlineData(8, 40, 15, 50, 50)]
        [InlineData(8, 40, 16, 0, 40)]
        [InlineData(8, 40, 16, 15, 25)]
        [InlineData(8, 40, 16, 30, 10)]
        [InlineData(8, 40, 16, 40, 10)]
        [InlineData(11, 50, 12, 0, 410)]
        [InlineData(11, 50, 12, 30, 410)]
        [InlineData(11, 50, 13, 0, 410)]
        [InlineData(11, 50, 13, 10, 400)]
        [InlineData(11, 50, 15, 50, 240)]
        [InlineData(11, 50, 16, 0, 230)]
        [InlineData(11, 50, 16, 15, 215)]
        [InlineData(11, 50, 16, 30, 200)]
        [InlineData(11, 50, 16, 40, 200)]
        [InlineData(12, 0, 12, 30, 420)]
        [InlineData(12, 0, 13, 0, 420)]
        [InlineData(12, 0, 13, 10, 410)]
        [InlineData(12, 0, 15, 50, 250)]
        [InlineData(12, 0, 16, 0, 240)]
        [InlineData(12, 0, 16, 15, 225)]
        [InlineData(12, 0, 16, 30, 210)]
        [InlineData(12, 0, 16, 40, 210)]
        [InlineData(12, 30, 13, 0, 420)]
        [InlineData(12, 30, 13, 10, 410)]
        [InlineData(12, 30, 15, 50, 250)]
        [InlineData(12, 30, 16, 0, 240)]
        [InlineData(12, 30, 16, 15, 225)]
        [InlineData(12, 30, 16, 30, 210)]
        [InlineData(12, 30, 16, 40, 210)]
        [InlineData(13, 0, 13, 10, 410)]
        [InlineData(13, 0, 15, 50, 250)]
        [InlineData(13, 0, 16, 0, 240)]
        [InlineData(13, 0, 16, 15, 225)]
        [InlineData(13, 0, 16, 30, 210)]
        [InlineData(13, 0, 16, 40, 210)]
        [InlineData(13, 10, 15, 50, 260)]
        [InlineData(13, 10, 16, 0, 250)]
        [InlineData(13, 10, 16, 15, 235)]
        [InlineData(13, 10, 16, 30, 220)]
        [InlineData(13, 10, 16, 40, 220)]
        [InlineData(15, 50, 16, 0, 410)]
        [InlineData(15, 50, 16, 15, 395)]
        [InlineData(15, 50, 16, 30, 380)]
        [InlineData(15, 50, 16, 40, 380)]
        [InlineData(16, 0, 16, 15, 405)]
        [InlineData(16, 0, 16, 30, 390)]
        [InlineData(16, 0, 16, 40, 390)]
        [InlineData(16, 15, 16, 30, 405)]
        [InlineData(16, 15, 16, 40, 405)]
        [InlineData(16, 30, 16, 40, 420)]
        #endregion
        public void WorkingHoursValidator_Special2_ME_LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime)
        {
            LackingTime(checkInHour, checkInMinute, checkOutHour, checkOutMinute, lackingTime, _special2_me_Group);
        }

        #endregion

        #region Helpers

        private void LackingTime(int checkInHour, int checkInMinute, int checkOutHour, int checkOutMinute, int lackingTime, WorkingPoliciesGroup group)
        {
            var date = new DateTime(2015, 1, 1);

            var user = new UserInfo()
            {
                WorkingPoliciesGroup = group,
                DailyRecords = new DailyWorkingRecord[]
                {
                    new DailyWorkingRecord { WorkingDay = date, CheckIn = date.AddHours(checkInHour).AddMinutes(checkInMinute), CheckOut = date.AddHours(checkOutHour).AddMinutes(checkOutMinute),
                    CheckInOutRecords = new List<CheckInOutRecord>()
                    {
                        new CheckInOutRecord()
                        {
                            CheckTime = date.AddHours(checkInHour).AddMinutes(checkInMinute)
                        },
                        new CheckInOutRecord()
                        {
                            CheckTime = date.AddHours(checkOutHour).AddMinutes(checkOutMinute)
                        }
                    }
                    }
                }
            };

            var validator = new WorkingHoursValidator();

            Assert.Equal(lackingTime, validator.Validate(user, date));
        }

        #endregion
    }
}
