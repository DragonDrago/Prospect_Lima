<template>
  <div class="page-not-found">
    <h1>404 page not found</h1>
    <router-link to="/">Back to homepage</router-link>
  </div>
</template>
<style scoped lang="scss">
.page-not-found {
  height: 80vh;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  h1 {
    color: #ed5564;
    margin-bottom: 1rem;
  }
  a {
    padding: 10px 15px;
    background: #336cfb;
    color: white;
    display: block;
    border-radius: 20px;
  }
}
</style>
