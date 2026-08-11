<template>
    <router-link class="book book-link" v-if="post && post.discussionId"
                 :to="{ name: 'singlepost', params: { discussionId: post.discussionId, postId: post.postId } }">
        <div class="book-top">
            <div class="book-cover" v-if="post.imageUrl">
                <img :src="post.imageUrl" :alt="post.title">
            </div>
            <div>
                <h2 class="book-title">{{ post.title }} </h2>
                <p class="book-author">{{ post.author }} </p>
            </div>
        </div>
    </router-link>
</template>

<script>
export default {
  name: "BookPostPreview",
  props: {
    post: {
      type: Object,
      required: true,
    }
  }
}
</script>

<style scoped>
.book-link {
  display: block;
  text-decoration: none;
  color: inherit;
  width: 100%;
  max-width: 200px;
}t

.book-top {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.book-cover {
  float: none;
  margin: 0 0 calc(var(--spacer) * 0.4) 0;
  padding: 0;
}

.book-title {
  font-size: 1rem;
  font-weight: bold;
  color: var(--c-accent);
  text-align: center;
  margin-top: var(--spacer);
  margin-bottom: calc(var(--spacer) / 2);
  padding-right: 0;
}

.book-author {
  font-size: 0.875rem;
  text-align: center;
}
</style>