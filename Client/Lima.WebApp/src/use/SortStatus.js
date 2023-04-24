export function sortStatus(status) {
  switch (status) {
    case 1:
      return "planned";
    case 2:
      return "expired";
    case 3:
      return "conducted";
  }
}
export function sortStatusShipping(status) {
  switch (status) {
    case 1:
      return "expectation";
    case 2:
      return "expired";
    case 3:
      return "confirmed";
  }
}
export function sortStatusBooking(status) {
  switch (status) {
    case 1:
      return "expectation";
    case 2:
      return "confirmed";
    case 3:
      return "expired";
  }
}
export function sortStatusBookingClasses(status) {
  switch (status) {
    case 1:
      return "text-blue";
    case 2:
      return "text-green";
    case 3:
      return "text-red";
  }
}
export function sortStatusClass(status) {
  switch (status) {
    case 1:
      return "text-blue";
    case 2:
      return "text-red";
    case 3:
      return "text-green";
  }
}
