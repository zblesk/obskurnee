<template>
    <div  class="book">
      <div class="book__cover" v-if="post.imageUrl">
        <a :href="post.url" class="book__link">
          <img :src="post.imageUrl" :alt="post.title">
        </a>
      </div>
      <a :href="post.url" class="book__link"><h2 class="book__title">{{ post.title }}</h2></a>
      <p class="book__pages" v-if="post.pageCount">{{ post.pageCount }} strán</p>
      <p class="book__owner">Navrhl(a) {{ post.ownerName }}</p>
      <div class="book__pitch" v-html="post.renderedText"> </div>
    </div>
</template>


<script>
    export default {
        name: "BookPost",
        props: ['post']
    }
</script>


<style scoped>

  .book {
    padding: var(--spacer);
    background-color: var(--c-bckgr-primary);
  }

  .book__cover {
    width: 100%;
    max-width: 150px;
    margin: 0 auto;
  }

  @media screen and (min-width: 400px) {
    .book__cover {
      float: left;
      margin: 0 var(--spacer) var(--spacer) 0;
    }
  }

  .book__cover img {
    width: 100%;
  }

  .book__link {
    text-decoration: none;
  }

  .book__title {
    font-size: 1.25em;
    color: var(--c-accent);
    margin: var(--spacer) 0 calc(var(--spacer)/2) 0;
  }

  @media screen and (min-width: 400px) {
    .book__title {
      margin-top: 0;
    }
  }

  .book__pages {
    margin-bottom: calc(var(--spacer) / 2);
    font-size: 0.875em;
  }

  .book__owner {
    margin-bottom: var(--spacer);
    font-size: 0.875em;
  }

</style>

<style>

  .book__pitch {
    line-height: 1.5;
  }

  .book__pitch p:last-child {
    margin-bottom: 0;
  }

  .book__pitch h2 {
    font-size: 1em;
    margin: 0 0 0.5em 0;
  }

</style>