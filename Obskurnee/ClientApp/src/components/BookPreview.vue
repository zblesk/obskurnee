<template>
 <div class="card">
  <span v-if="this.$slots.default"><slot></slot></span>
  <img :src="post.imageUrl" :alt="post.bookTitle" style="width:100%" v-if="post.imageUrl">
  <div class="container">
    <h4><b>{{ post.bookTitle }}</b></h4>
    <p>{{ post.author }}</p>
  </div>
</div> 
</template>


<script>
export default {
    name: "BookPreview",
    props: ['post']
}
</script>

<style scoped>
.card {
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
  max-width:  220px;
  padding: 0 1ex 0 1ex;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
}

.container {
  padding: 2px 16px;
}
</style>

