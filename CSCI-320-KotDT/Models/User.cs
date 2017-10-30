namespace CSCI_320_KotDT.Models {
    public class User {
        
        public string username { get; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public User(string username, string firstName, string lastName) {
            this.username = username;
            this.firstName = firstName;
            this.lastName = lastName;
        }
    }
}