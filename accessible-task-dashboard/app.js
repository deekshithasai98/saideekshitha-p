const form = document.getElementById("task-form");
const input = document.getElementById("task-title");
const list = document.getElementById("task-list");
const filter = document.getElementById("task-filter");
const status = document.getElementById("status-message");
const themeToggle = document.getElementById("theme-toggle");

let tasks = [];
let nextId = 1;

function renderTasks() {
  const visibleTasks = tasks.filter(task => {
    if (filter.value === "open") return !task.completed;
    if (filter.value === "done") return task.completed;
    return true;
  });

  list.replaceChildren();

  if (visibleTasks.length === 0) {
    const item = document.createElement("li");
    item.textContent = tasks.length
      ? "No tasks match this filter."
      : "No tasks yet. Add your first task above.";
    list.appendChild(item);
    return;
  }

  visibleTasks.forEach(task => {
    const item = document.createElement("li");
    const checkbox = document.createElement("input");
    checkbox.type = "checkbox";
    checkbox.checked = task.completed;
    checkbox.id = task-${task.id};
    checkbox.setAttribute("aria-label", Complete ${task.title});

    checkbox.addEventListener("change", () => {
      task.completed = checkbox.checked;
      status.textContent = `${task.title} ${
        task.completed ? "completed" : "reopened"
      }.`;
      renderTasks();
    });

    const label = document.createElement("label");
    label.htmlFor = checkbox.id;
    label.textContent = ` ${task.title} `;

    const removeButton = document.createElement("button");
    removeButton.type = "button";
    removeButton.textContent = "Remove";
    removeButton.setAttribute("aria-label", Remove ${task.title});

    removeButton.addEventListener("click", () => {
      tasks = tasks.filter(current => current.id !== task.id);
      status.textContent = Removed ${task.title}.;
      renderTasks();
      input.focus();
    });

    item.append(checkbox, label, removeButton);
    list.appendChild(item);
  });
}

form.addEventListener("submit", event => {
  event.preventDefault();
  const title = input.value.trim();

  if (!title) {
    input.focus();
    return;
  }

  tasks.push({
    id: nextId++,
    title,
    completed: false
  });

  input.value = "";
  status.textContent = Added ${title}.;
  renderTasks();
  input.focus();
});

filter.addEventListener("change", renderTasks);

themeToggle.addEventListener("click", () => {
  const darkMode = document.body.classList.toggle("dark");
  themeToggle.textContent = darkMode ? "Light mode" : "Dark mode";
  themeToggle.setAttribute("aria-pressed", String(darkMode));
});
