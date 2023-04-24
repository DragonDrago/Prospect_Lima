const EmptyComponentToggle = {
  data: () => ({
    isEmpty: false,
  }),
  mounted() {
    setTimeout(() => {
      return (this.isEmpty = true);
    }, 1000);
  },
  unmounted() {
    this.isEmpty = false;
  },
};

export default EmptyComponentToggle;
