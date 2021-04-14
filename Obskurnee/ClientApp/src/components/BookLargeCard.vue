<template>
  <div class="card">
    <div class="cover">
      <!--<router-link :to="{ name: 'post', params: { postId: post.postId } }">-->
        <img :src="post.imageUrl" :alt="post.title" v-if="post.imageUrl">
      <!--</router-link>-->
    </div>

    <h1 class="title">
      <!--<router-link :to="{ name: 'post', params: { postId: post.postId } }">-->{{ post.author }}: {{ post.title }}<!--</router-link>-->
      <a :href="post.url" :alt="post.title">
        <span class="goodreads-wrapper">
          <img src="../assets/goodreads_icon_32x32.png" alt="goodreads icon" class="goodreads-icon">
        </span>
      </a>
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
    line-height: 1.5;
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
      text-align: left;
    }
  }

  .summary {
    font-size: 1em;
  }

  .goodreads-wrapper {
    display: inline-block;
    width: 1em;
    height: 1em;
    margin-left: 0.5em;
  }

  .goodreads-icon {
    max-width: 100%;
    vertical-align: baseline;
  }

</style>


<script>
    export default {
        name: "BookLargeCard",
        props: ['post'],
    }
</script>