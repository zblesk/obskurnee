<template>
<section>
  <div v-if="post" class="wrapper">
    <book-post v-bind:post="post"></book-post>
    <div class="buttons buttons--padding">
      <router-link v-if="post.discussionId"
        :to="{ name: 'discussion', params: { discussionId: post.discussionId } }" >
          <button class="button-primary">◀ {{ $t("discussion.backToSuggestions")}}</button>
      </router-link>
    </div>
  </div>
</section>
</template>

<script>
import { mapActions } from "vuex";
import BookPost from "../BookPost.vue";

export default {
  name: 'SinglePost',
  components: { BookPost },
  data() {
      return {
        discussionId: 0,
        postId: 0,
        post: {},
      }
  },
  methods: {
    ...mapActions("discussions", ["getDiscussionPost"]),
  },
  async mounted() {
    this.discussionId = this.$route.params.discussionId;
    this.postId = this.$route.params.postId;
    this.post = await this.getDiscussionPost({ discussionId: this.discussionId, postId: this.postId });
  }
}
</script>