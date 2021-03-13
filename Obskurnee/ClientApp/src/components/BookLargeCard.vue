<template>
  <div class="card">
    <div class="cover">
      <a :href="post.url" :alt="post.title">
        <img :src="post.imageUrl" :alt="post.title" v-if="post.imageUrl">
      </a>
    </div>
    <h1 class="title">
      <a :href="post.url" :alt="post.title">{{ post.author }}: {{ post.title }}</a>
    </h1>
    <p class="summary" v-html="post.renderedText"></p>
  </div>
</template>

<style scoped>

  .card {
    padding: var(--spacer);
    background-color: var(--c-bckgr-primary);
    max-width: 800px;
    overflow: hidden;
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

  .cover img {
    max-width: 100%;
  }

  .title {
    font-size: 1.5em;
    font-weight: bold;
    text-align: center;
    margin-top: var(--spacer);
    margin-bottom: var(--spacer);
  }

  .title a {
    text-decoration: none;
  }

  @media screen and (min-width: 576px) {
    .title {
      margin-top: 0;
    }
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