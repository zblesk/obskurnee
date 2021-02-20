<template>
  <div class="card">
    <div class="cover">
      <img :src="post.imageUrl" :alt="post.title" v-if="post.imageUrl">
    </div>
    <h1 class="title">{{ post.author }}: {{ post.title }}</h1>
    <h2 class="heading">Prvotní návrh:</h2>
    <p class="summary" v-html="post.renderedText"></p>
  </div>
</template>

<style scoped>

  .card {
    padding: var(--spacer);
    margin: calc(var(--spacer) * 2) var(--spacer) 0 var(--spacer);
    background-color: var(--c-bckgr-primary);
    max-width: 800px;
  }

  @media screen and (min-width: 840px) {
    .card {
      margin: calc(var(--spacer) * 2) auto;
    }
  }

  .cover {
    width: 100%;
    max-width: 250px;
    margin: 0 auto var(--spacer) auto;
  }

  @media screen and (min-width: 576px) {
    .cover {
      float: left;
      margin-right: var(--spacer);
    }
  }

  .book__cover img {
    width: 100%;
    height: auto;
  }

  .title {
    font-size: 1.5em;
    font-weight: bold;
    text-align: center;
    margin-top: var(--spacer);
    margin-bottom: var(--spacer);
  }

  .heading {
    font-size: 1em;
    font-weight: bold;
  }

  .summary {
    font-size: 1em;
  }



</style>


<script>
    export default {
        name: "BookLargeCard",
        props: ['post'],
    }
</script>