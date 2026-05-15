// Data baked from MCP server (company-dep-mcp) on 2026-05-15

const users = [
  { employeeCode: "EMP001", firstName: "Laura",      lastName: "Bianchi", jobTitle: "IT Manager",              departmentName: "IT",              location: "Milano",  isActive: true, hireDate: "2018-03-12", managerEmployeeCode: null     },
  { employeeCode: "EMP002", firstName: "Marco",      lastName: "Rossi",   jobTitle: "Senior Backend Developer", departmentName: "IT",              location: "Roma",    isActive: true, hireDate: "2020-06-01", managerEmployeeCode: "EMP001" },
  { employeeCode: "EMP003", firstName: "Giulia",     lastName: "Verdi",   jobTitle: "DevOps Engineer",          departmentName: "IT",              location: "Torino",  isActive: true, hireDate: "2021-09-15", managerEmployeeCode: "EMP001" },
  { employeeCode: "EMP004", firstName: "Alessandro", lastName: "Neri",    jobTitle: "Security Manager",         departmentName: "Cybersecurity",   location: "Milano",  isActive: true, hireDate: "2017-11-20", managerEmployeeCode: null     },
  { employeeCode: "EMP005", firstName: "Sara",       lastName: "Conti",   jobTitle: "Security Analyst",         departmentName: "Cybersecurity",   location: "Roma",    isActive: true, hireDate: "2022-01-10", managerEmployeeCode: "EMP004" },
  { employeeCode: "EMP006", firstName: "Davide",     lastName: "Moretti", jobTitle: "SOC Specialist",           departmentName: "Cybersecurity",   location: "Napoli",  isActive: true, hireDate: "2023-04-03", managerEmployeeCode: "EMP004" },
  { employeeCode: "EMP007", firstName: "Francesca",  lastName: "Galli",   jobTitle: "HR Manager",               departmentName: "Human Resources", location: "Milano",  isActive: true, hireDate: "2016-02-18", managerEmployeeCode: null     },
  { employeeCode: "EMP008", firstName: "Elena",      lastName: "Marini",  jobTitle: "Recruiter",                departmentName: "Human Resources", location: "Roma",    isActive: true, hireDate: "2021-05-21", managerEmployeeCode: "EMP007" },
  { employeeCode: "EMP009", firstName: "Paolo",      lastName: "Ferrari", jobTitle: "Finance Manager",          departmentName: "Finance",         location: "Milano",  isActive: true, hireDate: "2015-07-01", managerEmployeeCode: null     },
  { employeeCode: "EMP010", firstName: "Marta",      lastName: "Ricci",   jobTitle: "Accountant",               departmentName: "Finance",         location: "Bologna", isActive: true, hireDate: "2022-10-10", managerEmployeeCode: "EMP009" },
  { employeeCode: "EMP011", firstName: "Luca",       lastName: "Romano",  jobTitle: "Operations Manager",       departmentName: "Operations",      location: "Torino",  isActive: true, hireDate: "2019-01-14", managerEmployeeCode: null     },
];

const departments = [
  { departmentName: "IT",              costCenter: "CC-IT-001",  activeUsers: 3, inactiveUsers: 0, totalUsers: 3 },
  { departmentName: "Cybersecurity",   costCenter: "CC-SEC-001", activeUsers: 3, inactiveUsers: 0, totalUsers: 3 },
  { departmentName: "Finance",         costCenter: "CC-FIN-001", activeUsers: 2, inactiveUsers: 0, totalUsers: 2 },
  { departmentName: "Human Resources", costCenter: "CC-HR-001",  activeUsers: 2, inactiveUsers: 0, totalUsers: 2 },
  { departmentName: "Operations",      costCenter: "CC-OPS-001", activeUsers: 1, inactiveUsers: 1, totalUsers: 2 },
];

const deptMeta = {
  "IT":              { color: "primary", icon: "bi-pc-display"    },
  "Cybersecurity":   { color: "danger",  icon: "bi-shield-lock"   },
  "Finance":         { color: "success", icon: "bi-currency-euro" },
  "Human Resources": { color: "info",    icon: "bi-person-heart"  },
  "Operations":      { color: "warning", icon: "bi-gear"          },
};

function getMeta(dept) {
  return deptMeta[dept] || { color: "secondary", icon: "bi-building" };
}

// ── Summary Cards ──────────────────────────────────────────────────────────────

function renderSummaryCards() {
  const activeCount = users.filter(u => u.isActive).length;
  const locationCount = new Set(users.map(u => u.location)).size;

  const cards = [
    { label: "Total Employees", value: users.length,      icon: "bi-people-fill",       color: "primary" },
    { label: "Active",          value: activeCount,        icon: "bi-person-check-fill", color: "success" },
    { label: "Departments",     value: departments.length, icon: "bi-diagram-3-fill",    color: "info"    },
    { label: "Locations",       value: locationCount,      icon: "bi-geo-alt-fill",      color: "warning" },
  ];

  document.getElementById("summary-cards").innerHTML = cards.map(c => `
    <div class="col-6 col-lg-3">
      <div class="card shadow-sm summary-card p-3">
        <div class="d-flex align-items-center gap-3">
          <div class="summary-icon bg-${c.color} bg-opacity-10 text-${c.color}">
            <i class="bi ${c.icon} fs-4"></i>
          </div>
          <div>
            <div class="fs-1 fw-bold lh-1">${c.value}</div>
            <div class="text-muted small mt-1">${c.label}</div>
          </div>
        </div>
      </div>
    </div>
  `).join("");
}

// ── Department Cards ───────────────────────────────────────────────────────────

function renderDeptCards() {
  document.getElementById("dept-cards").innerHTML = departments.map(dept => {
    const { color, icon } = getMeta(dept.departmentName);
    const pct = Math.round((dept.activeUsers / dept.totalUsers) * 100);
    return `
      <div class="col-sm-6 col-lg-4">
        <div class="card shadow-sm dept-card h-100 p-3">
          <div class="d-flex align-items-center gap-2 mb-3">
            <div class="dept-icon bg-${color} bg-opacity-10 text-${color}">
              <i class="bi ${icon}"></i>
            </div>
            <div>
              <div class="fw-semibold">${dept.departmentName}</div>
              <code class="text-muted small">${dept.costCenter}</code>
            </div>
          </div>
          <div class="row text-center g-0 mb-2 border rounded">
            <div class="col py-2 border-end">
              <div class="fw-bold text-${color}">${dept.activeUsers}</div>
              <div class="text-muted" style="font-size:.72rem">Active</div>
            </div>
            <div class="col py-2 border-end">
              <div class="fw-bold text-danger">${dept.inactiveUsers}</div>
              <div class="text-muted" style="font-size:.72rem">Inactive</div>
            </div>
            <div class="col py-2">
              <div class="fw-bold">${dept.totalUsers}</div>
              <div class="text-muted" style="font-size:.72rem">Total</div>
            </div>
          </div>
          <div class="progress mt-auto" style="height:5px">
            <div class="progress-bar bg-${color}" style="width:${pct}%"
                 role="progressbar" aria-valuenow="${pct}" aria-valuemin="0" aria-valuemax="100"></div>
          </div>
          <div class="text-end text-muted mt-1" style="font-size:.7rem">${pct}% active headcount</div>
        </div>
      </div>
    `;
  }).join("");
}

// ── Users Table ────────────────────────────────────────────────────────────────

function renderUsersTable(filter = "") {
  const term = filter.trim().toLowerCase();
  const list = term
    ? users.filter(u =>
        `${u.firstName} ${u.lastName}`.toLowerCase().includes(term) ||
        u.jobTitle.toLowerCase().includes(term) ||
        u.departmentName.toLowerCase().includes(term) ||
        u.employeeCode.toLowerCase().includes(term) ||
        u.location.toLowerCase().includes(term)
      )
    : [...users];

  const tbody = document.getElementById("users-tbody");

  if (!list.length) {
    tbody.innerHTML = `<tr><td colspan="7" class="text-center text-muted py-5">No employees match your search.</td></tr>`;
    return;
  }

  tbody.innerHTML = list.map(u => {
    const { color } = getMeta(u.departmentName);
    const date = new Date(u.hireDate + "T00:00:00").toLocaleDateString("it-IT");
    const statusBadge = u.isActive
      ? `<span class="badge bg-success-subtle text-success fw-medium">Active</span>`
      : `<span class="badge bg-secondary-subtle text-secondary fw-medium">Inactive</span>`;
    return `
      <tr>
        <td><code class="text-muted small">${u.employeeCode}</code></td>
        <td><span class="fw-semibold">${u.firstName} ${u.lastName}</span></td>
        <td class="text-muted">${u.jobTitle}</td>
        <td><span class="badge dept-badge rounded-pill bg-${color} bg-opacity-10 text-${color}">${u.departmentName}</span></td>
        <td class="text-muted"><i class="bi bi-geo-alt me-1"></i>${u.location}</td>
        <td class="text-muted small">${date}</td>
        <td>${statusBadge}</td>
      </tr>
    `;
  }).join("");
}

// ── Manager / Direct Reports ───────────────────────────────────────────────────

function renderManagerCards() {
  const managers = users.filter(m =>
    users.some(u => u.managerEmployeeCode === m.employeeCode)
  );

  document.getElementById("manager-cards").innerHTML = managers.map(mgr => {
    const reports = users.filter(u => u.managerEmployeeCode === mgr.employeeCode);
    const { color } = getMeta(mgr.departmentName);
    const initials = mgr.firstName[0] + mgr.lastName[0];

    const reportItems = reports.map(r => `
      <div class="d-flex align-items-center gap-2 py-2 border-bottom">
        <div class="avatar-sm rounded-circle bg-${color} bg-opacity-10 text-${color}
                    d-flex align-items-center justify-content-center fw-bold">
          ${r.firstName[0]}${r.lastName[0]}
        </div>
        <div class="overflow-hidden">
          <div class="fw-semibold small text-truncate">${r.firstName} ${r.lastName}</div>
          <div class="text-muted text-truncate" style="font-size:.72rem">
            ${r.jobTitle} &middot; <i class="bi bi-geo-alt"></i>${r.location}
          </div>
        </div>
      </div>
    `).join("");

    return `
      <div class="col-sm-6 col-xl-4">
        <div class="card shadow-sm h-100" style="border-top:3px solid var(--bs-${color})">
          <div class="card-body">
            <div class="d-flex align-items-center gap-3 mb-3">
              <div class="avatar-md rounded-circle bg-${color} text-white
                          d-flex align-items-center justify-content-center fw-bold">
                ${initials}
              </div>
              <div>
                <div class="fw-bold">${mgr.firstName} ${mgr.lastName}</div>
                <div class="text-muted small">${mgr.jobTitle}</div>
                <span class="badge bg-${color} bg-opacity-10 text-${color} mt-1"
                      style="font-size:.65rem">${mgr.departmentName}</span>
              </div>
            </div>
            <div class="text-muted fw-semibold mb-2"
                 style="font-size:.7rem;letter-spacing:.06em;text-transform:uppercase">
              ${reports.length} Direct Report${reports.length !== 1 ? "s" : ""}
            </div>
            ${reportItems}
          </div>
        </div>
      </div>
    `;
  }).join("");
}

// ── Search ─────────────────────────────────────────────────────────────────────

document.getElementById("search-input").addEventListener("input", e => {
  renderUsersTable(e.target.value);
});

// ── Init ───────────────────────────────────────────────────────────────────────

renderSummaryCards();
renderDeptCards();
renderUsersTable();
renderManagerCards();
