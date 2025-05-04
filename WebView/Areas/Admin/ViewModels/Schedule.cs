    namespace WebView.Areas.Admin.ViewModels
    {
        public class Schedule
        {
            public int Id_NgayLamViec { get; set; } // ID ngày làm việc 
            public DateTime Date { get; set; } // Ngày
            public bool status { get; set; } // Trạng thái
            public List<Shift> Shifts { get; set; } // Danh sách các ca trong ngày
        }

        // ViewModel đại diện cho từng ca làm việc
        public class Shift
        {
            public int Id { get; set; } // ID ca làm việc   
            public string ShiftName { get; set; } // Tên ca
            public TimeSpan StartTime { get; set; } // Giờ bắt đầu
            public TimeSpan EndTime { get; set; } // Giờ kết thúc
            public List<Employee> Employees { get; set; } // Danh sách nhân viên
        }

        public class Employee
        {
            public int Id { get; set; } // ID nhân viên 
            public string Name { get; set; } // Tên nhân viên
            public string Role { get; set; } // Chức vụ
        }

    }
