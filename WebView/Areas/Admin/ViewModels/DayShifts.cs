namespace WebView.Areas.Admin.ViewModels
{
    public class DayShifts
    {
        public DateTime Date { get; set; } // Ngày
        public List<Shift> Shifts { get; set; } // Danh sách các ca trong ngày
    }

    // ViewModel đại diện cho từng ca làm việc
    public class Shift
    {
        public string ShiftName { get; set; } // Tên ca
        public string StartTime { get; set; } // Giờ bắt đầu
        public string EndTime { get; set; } // Giờ kết thúc
        public List<Employee> Employees { get; set; } // Danh sách nhân viên
    }

    public class Employee
    {
        public string Name { get; set; } // Tên nhân viên
        public string Role { get; set; } // Chức vụ
    }

}
