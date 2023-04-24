import store from "../store";

let tableValue = {};

export const editTable = (id) => {
  const tableRows = document.querySelectorAll(`#table_body_${id} td`);
  const table = document.querySelectorAll(".table > tbody > tr");
  removeChanges(table);
  addChanges(tableRows, id);
};

const addChanges = (table, id) => {
  table.forEach((td) => {
    td.setAttribute("contenteditable", "true");
    td.style.borderBottom = "1px solid #336cfb";
    td.addEventListener("input", (e) => getTableValue(e, id));
  });
  table[0].focus();
};

export const getTableValue = (e, id) => {
  const { textContent } = e.target;
  tableValue = {
    ...tableValue,
    id,
    [e.target.getAttribute("name")]: textContent,
  };
  store.commit("SET_TABLE_VALUE", tableValue);
};

const removeChanges = (table) => {
  table.forEach((tr) => {
    const tableRows = tr.querySelectorAll("td");
    tableRows.forEach((td) => {
      td.removeAttribute("contenteditable", "true");
      td.style.borderBottom = null;
    });
    tableValue = {};
  });
};
