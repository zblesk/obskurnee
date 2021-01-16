<template>
<div class="grid">
</div>
</template>

<script>
export default {
  name: 'Home',
  props: {
    
  }
}
</script>

<style scoped>
.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(100px, 400px));
  gap: 10px;
  margin: 0;
  background-color: gray;
  font-family: "Raleway", sans-serif;
}
</style>