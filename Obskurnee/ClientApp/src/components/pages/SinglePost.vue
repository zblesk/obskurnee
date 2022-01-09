<template>
<section>
  <router-link v-if="post.discussionId"
    :to="{ name: 'discussion', params: { discussionId: post.discussionId } }" >
      ◀ {{ $t("discussion.backToSuggestions")}}
  </router-link>
  <div v-if="post" class="wrapper">
    <book-post v-bind:post="post"></book-post>
    <div class="buttons buttons--padding">
      <button class="button-primary">{{ $t("discussion.backToSuggestions")}}</button>
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