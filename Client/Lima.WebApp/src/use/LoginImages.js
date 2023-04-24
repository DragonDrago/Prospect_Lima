export default function moveLoginImages() {
  document.querySelector(".login-right").addEventListener("mousemove", (e) => {
    const images = document.querySelectorAll(".images");
    images.forEach((image) => {
      let x = image.getBoundingClientRect().left + image.clientWidth / 2;
      let y = image.getBoundingClientRect().top + image.clientHeight / 2;

      image.style.transform = `translate(${(e.pageX - x) / 12}px, ${
        (e.pageY - y) / 12
      }px)`;
    });
  });
}
