
const BASE_URL = "https://localhost:7141/api/Employee";

window.onload = async function () {
    const employeeId = getEmployeeIdFromUrl();
    if (!employeeId) return alert("Invalid employee ID.");

    await loadCountries();
    await loadLanguages();
    await loadEmployeeData(employeeId);
};

function getEmployeeIdFromUrl() {
    const params = new URLSearchParams(window.location.search);
    return params.get("employeeId");
}

async function loadCountries() {
    const res = await fetch(`${BASE_URL}/GetAllCountries`);
    const countries = await res.json();
    const countrySelect = document.getElementById('country');
    countrySelect.innerHTML = '<option value="">Select Country</option>';
    countries.forEach(c => {
        const option = document.createElement('option');
        option.value = c.countryId;
        option.text = c.countryName;
        countrySelect.appendChild(option);
    });
}

async function loadStates(countryId) {
    const res = await fetch(`${BASE_URL}/GetAllStates`);
    const states = await res.json();
    const stateSelect = document.getElementById('state');
    stateSelect.innerHTML = '<option value="">Select State</option>';
    states.filter(s => s.countryId == countryId).forEach(s => {
        const option = document.createElement('option');
        option.value = s.stateId;
        option.text = s.stateName;
        stateSelect.appendChild(option);
    });
}

async function loadCities(stateId) {
    const res = await fetch(`${BASE_URL}/GetAllCities`);
    const cities = await res.json();
    const citySelect = document.getElementById('city');
    citySelect.innerHTML = '<option value="">Select City</option>';
    cities.filter(c => c.stateId == stateId).forEach(c => {
        const option = document.createElement('option');
        option.value = c.cityId;
        option.text = c.cityName;
        citySelect.appendChild(option);
    });
}

function loadLanguages() {
    const languages = [
        { languageId: 1, languageName: "Telugu" },
        { languageId: 2, languageName: "English" },
        { languageId: 3, languageName: "French" },
        { languageId: 4, languageName: "Spanish" }
    ];
    const langDiv = document.getElementById('languages');
    languages.forEach(l => {
        const label = document.createElement('label');
        label.innerHTML = `<input type="checkbox" value="${l.languageId}" data-lang="${l.languageName}"> ${l.languageName}`;
        langDiv.appendChild(label);
        langDiv.appendChild(document.createElement('br'));
    });
}

document.getElementById('country').addEventListener('change', async function () {
    await loadStates(this.value);
});

document.getElementById('state').addEventListener('change', async function () {
    await loadCities(this.value);
});

document.getElementById('languages').addEventListener('change', function (e) {
    const langId = e.target.value;
    const langName = e.target.dataset.lang;
    const profDiv = document.getElementById('proficiencies');

    if (e.target.checked) {
        const div = document.createElement('div');
        div.classList.add('proficiency');
        div.id = `prof-${langId}`;
        div.innerHTML = `
            <label>${langName} Proficiency:</label><br>
            <label><input type="radio" name="prof-${langId}" value="Beginner" required> Beginner</label>
            <label><input type="radio" name="prof-${langId}" value="Intermediate"> Intermediate</label>
            <label><input type="radio" name="prof-${langId}" value="Expert"> Expert</label>
        `;
        profDiv.appendChild(div);
    } else {
        const toRemove = document.getElementById(`prof-${langId}`);
        if (toRemove) profDiv.removeChild(toRemove);
    }
});

async function loadEmployeeData(id) {
    const res = await fetch(`${BASE_URL}/GetEmployeeByID/${id}`);
    const emp = await res.json();

    const form = document.getElementById("editEmployeeForm");
    form.name.value = emp.name;
    form.dob.value = emp.date.split("T")[0];
    form.emailAddress.value = emp.emailAddress;
    form.address.value = emp.address;
    form.gender.value = emp.gender;

    form.country.value = emp.countryId;
    await loadStates(emp.countryId);
    form.state.value = emp.stateId;
    await loadCities(emp.stateId);
    form.city.value = emp.cityId;

    (emp.languages || []).forEach(lang => {
        const checkbox = document.querySelector(`#languages input[value="${lang.languageId}"]`);
        if (checkbox) {
            checkbox.checked = true;
            checkbox.dispatchEvent(new Event("change")); // trigger proficiency div
            const radio = document.querySelector(`input[name="prof-${lang.languageId}"][value="${lang.proficiency}"]`);
            if (radio) radio.checked = true;
        }
    });
}

document.getElementById("editEmployeeForm").addEventListener("submit", function (e) {
    e.preventDefault();
    const id = getEmployeeIdFromUrl();

    try {
        const form = e.target;

        const genderEl = document.getElementById("gender");
        const countryEl = document.getElementById("country");
        const stateEl = document.getElementById("state");
        const cityEl = document.getElementById("city");

        if (!genderEl || !countryEl || !stateEl || !cityEl) {
            alert("Form fields missing. Please check the form HTML IDs.");
            return;
        }

        const payload = {
            employeeId: parseInt(id),
            name: form.name.value.trim(),
            date: form.dob.value,
            emailAddress: form.emailAddress.value.trim(),
            address: form.address.value.trim(),
            gender: parseInt(genderEl.value),
            countryId: parseInt(countryEl.value),
            stateId: parseInt(stateEl.value),
            cityId: parseInt(cityEl.value),
            countryName: countryEl.selectedOptions[0].text,
            stateName: stateEl.selectedOptions[0].text,
            cityName: cityEl.selectedOptions[0].text,
            languages: []
        };

        document.querySelectorAll("#languages input[type=checkbox]:checked").forEach(langCb => {
            const langId = langCb.value;
            const langName = langCb.dataset.lang;
            const profRadio = document.querySelector(`input[name=prof-${langId}]:checked`);
            if (profRadio) {
                payload.languages.push({
                    languageId: parseInt(langId),
                    languageName: langName,
                    proficiency: profRadio.value
                });
            }
        });

        console.log("PAYLOAD BEING SENT:", payload);

        fetch(`${BASE_URL}/UpdateEmployee/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        })
            .then(async res => {
                const contentType = res.headers.get("content-type");
                const responseBody = contentType && contentType.includes("application/json")
                    ? await res.json()
                    : await res.text();

                if (!res.ok) throw new Error(responseBody.message || responseBody || res.statusText);

                alert("Employee updated successfully!");
                window.location.href = "DashBoard.html";
            })
            .catch(err => {
                console.error(err);
                alert("Update failed: " + err.message);
            });

    } catch (err) {
        console.error("Form submit error:", err);
        alert("Something went wrong: " + err.message);
    }
});
