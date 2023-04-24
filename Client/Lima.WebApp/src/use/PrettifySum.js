export function prettify(num) {
  if (typeof num === "number") {
    return num
      .toString()
      .replace(/(\d{1,3}(?=(?:\d\d\d)+(?!\d)))/g, "$1" + ".");
  } else {
    return num.replace(/(\d{1,3}(?=(?:\d\d\d)+(?!\d)))/g, "$1" + ".") || 0;
  }
}

export function isEmpty(item) {
  return item ? item : "Пусто";
}
