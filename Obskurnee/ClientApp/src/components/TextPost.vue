<template>
 <div class="card">
  <h2 v-if="$slots.default" class="slot"><slot></slot></h2>
  <div class="container">
    <h3 class="title">{{ post.title }}</h3>
    <p v-html="post.renderedText" class="desc"></p>
  </div>
</div> 
</template>


<script>
export default {
    name: "TextPost",
    props: ['post']
}
</script>

<style scoped>

  .slot {
    text-align: center;
    font-size: 1.25em;
    margin-top: 0;
    margin-bottom: var(--spacer);
  }

  .container {
    text-align: center;
  }

  .title {
    color: var(--c-primary);
    font-size: 1.25em;
  }


</style>

