<template>
    <div  class="book">
      <div class="book__cover" v-if="post.imageUrl">
        <a :href="post.url" class="book__link">
          <img :src="post.imageUrl" :alt="post.title">
        </a>
      </div>
      <a :href="post.url" class="book__link">
        <h2 class="book__title">{{ post.title }}</h2></a>
      <p class="book__pages" v-if="post.pageCount">{{ post.pageCount }} strán</p>
      <p class="book__owner">Navrhla {{ post.ownerName }}</p>
      <div class="book__text" v-html="post.renderedText"> </div>
    </div>
</template>


<script>
    export default {
        name: "BookPost",
        props: ['post']
    }
</script>


<style scoped>



</style>