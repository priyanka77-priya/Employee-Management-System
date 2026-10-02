// // Replace with your API base URL
// const BASE_URL = "https://localhost:7141/api/Employee";

// // Load Countries on page load
// window.onload = function() {
//     loadCountries();
//     loadLanguages();
// };

// function loadCountries() {
//     fetch(`${BASE_URL}/GetAllCountries`)
//         .then(res => res.json())
//         .then(data => {
//             const countrySelect = document.getElementById('country');
//             data.forEach(c => {
//                 const option = document.createElement('option');
//                 option.value = c.countryId;
//                 option.text = c.countryName;
//                 countrySelect.appendChild(option);
//             });
//         });
// }

// document.getElementById('country').addEventListener('change', function() {
//     const countryId = this.value;
//     const stateSelect = document.getElementById('state');
//     stateSelect.innerHTML = '<option value="">Select State</option>';
//     document.getElementById('city').innerHTML = '<option value="">Select City</option>';

//     fetch(`${BASE_URL}/GetAllStates`)
//         .then(res => res.json())
//         .then(states => {
//             states.filter(s => s.countryId == countryId).forEach(s => {
//                 const option = document.createElement('option');
//                 option.value = s.stateId;
//                 option.text = s.stateName;
//                 stateSelect.appendChild(option);
//             });
//         });
// });

// document.getElementById('state').addEventListener('change', function() {
//     const stateId = this.value;
//     const citySelect = document.getElementById('city');
//     citySelect.innerHTML = '<option value="">Select City</option>';

//     fetch(`${BASE_URL}/GetAllCities`)
//         .then(res => res.json())
//         .then(cities => {
//             cities.filter(c => c.stateId == stateId).forEach(c => {
//                 const option = document.createElement('option');
//                 option.value = c.cityId;
//                 option.text = c.cityName;
//                 citySelect.appendChild(option);
//             });
//         });
// });

// function loadLanguages() {
//     const languages = [
//         { languageId: 1, languageName: "Telugu" },
//         { languageId: 2, languageName: "English" },
//         { languageId: 3, languageName: "French" },
//         { languageId: 4, languageName: "Spanish" }
//     ];
//     const langDiv = document.getElementById('languages');
//     languages.forEach(l => {
//         const label = document.createElement('label');
//         label.innerHTML = `<input type="checkbox" value="${l.languageId}" data-lang="${l.languageName}"> ${l.languageName}`;
//         langDiv.appendChild(label);
//         langDiv.appendChild(document.createElement('br'));
//     });
// }

// document.getElementById('languages').addEventListener('change', function(e) {
//     const langId = e.target.value;
//     const langName = e.target.dataset.lang;
//     const profDiv = document.getElementById('proficiencyLevels');

//     if (e.target.checked) {
//         const div = document.createElement('div');
//         div.classList.add('proficiency');
//         div.id = `prof-${langId}`;
//         div.innerHTML = `
//             <label>${langName} Proficiency:</label><br>
//             <label><input type="radio" name="prof-${langId}" value="Beginner" required> Beginner</label>
//             <label><input type="radio" name="prof-${langId}" value="Intermediate"> Intermediate</label>
//             <label><input type="radio" name="prof-${langId}" value="Expert"> Expert</label>
//         `;
//         profDiv.appendChild(div);
//     } else {
//         const toRemove = document.getElementById(`prof-${langId}`);
//         if (toRemove) profDiv.removeChild(toRemove);
//     }
// });
// document.getElementById("cancelButton").addEventListener("click", function() {
//     console.log("Cancel button clicked!");
//     alert("Cancel button clicked!");
//     document.getElementById("employeeForm").reset();
// });

// document.getElementById("employeeForm").addEventListener("submit", function(e) {
//   e.preventDefault();

//   const form = e.target;
//   const payload = {
//     name: form.name.value.trim(),
//     date: form.dob.value,
//     emailAddress: form.emailAddress.value.trim(),
//     address: form.address.value.trim(),
//     gender: parseInt(form.gender.value),
//     countryId: parseInt(form.country.value),
//     stateId: parseInt(form.state.value),
//     cityId: parseInt(form.city.value),
//     languages: []
//   };

//  //Collect languages + proficiency
// document.querySelectorAll("#languages input[type=checkbox]:checked")
//   .forEach(langCb => {
//     const langId = langCb.value;
//     const langName = langCb.dataset.lang; // get langName from dataset

//     // Find the selected proficiency radio button for this language
//     const profRadio = document.querySelector(`input[name=prof-${langId}]:checked`);
//     if (profRadio) {
//       payload.languages.push({
//         languageId: parseInt(langId),
//         languageName: langName, 
//         proficiency: profRadio.value
//       });
//     }
//   });

//   // Send POST
//   fetch("https://localhost:7141/api/Employee/AddEmployee", {
//     method: "POST",
//     headers: { "Content-Type": "application/json" },
//     body: JSON.stringify(payload)
//   })
//   .then(async res => {
//     const json = await res.json();
//     if (!res.ok) throw new Error(json.message || res.statusText);
//     alert("Employee saved!");
//   })
//   .catch(err => {
//     console.error(err);
//     alert("Save failed: " + err.message);
//   });
// });



