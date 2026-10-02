window.onload = function () {
  const token = localStorage.getItem("token");
  if (!token) {
    alert("Please login first.");
    // window.location.href = "login.html";
    window.location.replace("login.html"); 
  }
};
history.pushState(null, null, location.href);
window.onpopstate = function () {
  history.go(1);  // prevents back navigation
};
const pageSize = 4; // Number of records per page
let currentPage = 1;
let totalRecords = 14;


const employeeTableBody = document.getElementById("employeeTableBody");
const prevBtn = document.getElementById("prevPage");
const nextBtn = document.getElementById("nextPage");
const currentPageSpan = document.getElementById("currentPage");
const totalPagesSpan = document.getElementById("totalPages");

const API_BASE_URL = "https://localhost:7141/api/Employee";

// Load the first page when DOM is ready
document.addEventListener("DOMContentLoaded", () => {
  fetchEmployees(currentPage);
});

// Navigate to Add Employee form
function navigateToForm() {
  window.location.href = "EmployeeForm.html";
}

// // Fetch employees with pagination
// async function fetchEmployees(pageNumber) {
//   try {
//     const response = await fetch(`${API_BASE_URL}/getall/${pageNumber}/${pageSize}`);
//     if (!response.ok) {
//       throw new Error("Failed to fetch employees.");
//     }

//     const result = await response.json();
//     totalRecords = result.totalRecords;
//     currentPage = result.pageNumber;

//     renderTable(result.data);
//     updatePaginationControls();
//   } catch (error) {
//     console.error("Error:", error);
//     alert(error.message);
//   }
// }
async function fetchEmployees(pageNumber) {
  const token = localStorage.getItem("token"); // get the token
console.log("Token sending to server:", token);
  try {
    const response = await fetch(`${API_BASE_URL}/getall/${pageNumber}/${pageSize}`, {
      method: "GET",
      headers: {
        "Authorization": `Bearer ${token}`,  // pass the token here
        "Content-Type": "application/json"
      }
    });

    if (!response.ok) {
      throw new Error("Unauthorized or failed to fetch employees.");
    }

    const result = await response.json();
    totalRecords = result.totalRecords;
    currentPage = result.pageNumber;

    renderTable(result.data);
    updatePaginationControls();
  } catch (error) {
    console.error("Error:", error);
    alert(error.message);
  }
}

// Render the employee table
function renderTable(employees) {
  employeeTableBody.innerHTML = '';

  if (!employees || employees.length === 0) {
    employeeTableBody.innerHTML = `<tr><td colspan="11">No employees found.</td></tr>`;
    return;
  }

  employees.forEach(emp => {
    const languages = (emp.languages || [])
      .map(lang => `${lang.languageName} (${lang.proficiency})`)
      .join(', ');

    const gender = getGenderText(emp.gender);

    const row = `
      <tr>
        <td>${emp.employeeId}</td>
        <td>${emp.name}</td>
        <td>${new Date(emp.date).toLocaleDateString()}</td>
        <td>${emp.emailAddress}</td>
        <td>${emp.address}</td>
        <td>${gender}</td>
        <td>${emp.countryName || '-'}</td>
        <td>${emp.stateName || '-'}</td>
        <td>${emp.cityName || '-'}</td>
        <td>${languages || '-'}</td>
        <td>
          <button class="action-btn edit-btn" onclick="editEmployee(${emp.employeeId})">Edit</button>
          <button class="action-btn view-btn" onclick="viewEmployee(${emp.employeeId})">View</button>
          <button class="action-btn delete-btn" onclick="SoftdeleteEmployee(${emp.employeeId})">SoftDelete</button>
          
           

          
        </td>
      </tr>
    `;
    employeeTableBody.insertAdjacentHTML('beforeend', row);
  });
}

// Update pagination controls
function updatePaginationControls() {
  const totalPages = Math.ceil(totalRecords / pageSize);
  currentPageSpan.textContent = currentPage;
  totalPagesSpan.textContent = totalPages;

  prevBtn.disabled = currentPage <= 1;
  nextBtn.disabled = currentPage >= totalPages;
}

// Event listeners for pagination buttons
prevBtn.addEventListener('click', () => {
  if (currentPage > 1) {
    fetchEmployees(currentPage - 1);
  }
});

nextBtn.addEventListener('click', () => {
  const totalPages = Math.ceil(totalRecords / pageSize);
  if (currentPage < totalPages) {
    fetchEmployees(currentPage + 1);
  }
});

// Gender display helper
function getGenderText(gender) {
  return gender === 1 ? "Male" : gender === 2 ? "Female" : "Other";
}

// Edit/View/Delete actions



function mapGender(gender) {
  switch (gender) {
    case 1: return "Male";
    case 2: return "Female";
    case 3: return "Other";
    default: return "Unknown";
  }
}

function viewEmployee(id) {
  fetch(`${API_BASE_URL}/GetEmployeeByID/${id}`)
    .then(res => {
      if (!res.ok) throw new Error("Failed to load employee details.");
      return res.json();
    })
    .then(emp => {
      const genderText = mapGender(emp.gender);
      const langs = (emp.languages || []).map(l => `${l.languageName} (${l.proficiency})`).join(", ");

      const html = `
        <p><strong>Name:</strong> ${emp.name}</p>
        <p><strong>Date of Birth:</strong> ${emp.date.split("T")[0]}</p>
        <p><strong>Email:</strong> ${emp.emailAddress}</p>
        <p><strong>Address:</strong> ${emp.address}</p>
        <p><strong>Gender:</strong> ${genderText}</p>
        <p><strong>Country:</strong> ${emp.countryName}</p>
        <p><strong>State:</strong> ${emp.stateName}</p>
        <p><strong>City:</strong> ${emp.cityName}</p>
        <p><strong>Languages:</strong> ${langs}</p>
      `;

      document.getElementById("employeeDetails").innerHTML = html;
      document.getElementById("viewModal").style.display = "block";
    })
    .catch(err => {
      console.error(err);
      alert("Failed to fetch employee details.");
    });
}


function closeModal() {
  document.getElementById("viewModal").style.display = "none";
}


function SoftdeleteEmployee(id) {
  if (confirm("Are you sure you want to soft delete this employee?")) {
    fetch(`${API_BASE_URL}/SoftDelete/${id}`, {
      method: 'PUT',
    })
    .then(response => {
      if (response.ok) {
        alert("Employee soft deleted successfully!");
        fetchEmployees(currentPage); // Reload current page
      } else {
        alert("Failed to soft delete employee.");
      }
    })
    .catch(error => {
      console.error("Error during soft delete:", error);
      alert("An error occurred while trying to soft delete the employee.");
    });
  }
}


function editEmployee(id) {
  window.location.href = `editEmployeeForm.html?employeeId=${id}`;
}
// // Logout button logic
// document.getElementById("logoutBtn").addEventListener("click", function () {
//   localStorage.clear(); // remove JWT or session data
//   sessionStorage.clear();
//  alert("You have been logged out.");
//   window.location.href = "login.html"; // redirect to login page
// });

/// Show modal when logout button clicked
document.getElementById("logoutBtn").addEventListener("click", function () {
  document.getElementById("logoutModal").style.display = "block";
});

// When user confirms logout
document.getElementById("confirmLogout").addEventListener("click", function () {
  localStorage.clear();
  sessionStorage.clear();
  window.location.href = "login.html";
});

// When user cancels logout
document.getElementById("cancelLogout").addEventListener("click", function () {
  document.getElementById("logoutModal").style.display = "none";
});
