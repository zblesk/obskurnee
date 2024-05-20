<template>
    <div class="book" v-if="post && post.discussionId">
        <div class="book__top">
            <div class="book__cover" v-if="post.imageUrl">
                <a :href="post.url" class="book__link">
                    <img :src="post.imageUrl" :alt="post.title">
                </a>
            </div>
            <router-link class="book__permalink"
                         :to="{ name: 'singlepost', params: { discussionId: post.discussionId, postId: post.postId } }">
                🔗
            </router-link>
            <div>
                <a :href="post.url" class="book__link">
                    <h2 class="book__title" v-html="post.title"></h2>
                </a>
                <p v-if="post.author" class="book__author" v-html="post.author"></p>
                <p class="book__pages" v-if="post.pageCount">{{$t('bookpost.numPages', [post.pageCount])}}</p>
                <p class="book__owner">{{$t('bookpost.suggestedBy')}} <user-link :userId="post.ownerId" :userName="post.ownerName" /></p>
                <div class="book__text" v-html="post.renderedText"> </div>
            </div>
        </div>
        <div class="book__repost" v-if="isRepostingAllowed
        && activeDiscussionId != post.discussionId
        && topic == 'Books'">
            <router-link :to="{ name: 'discussion', params: { discussionId: activeDiscussionId },
          query: { parentPostId: post.postId, fromDiscussionId: post.discussionId } }">
                <button class="button-secondary button-repost">{{$t('bookpost.addToOngoing')}}</button>
            </router-link>
        </div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex'
import UserLink from './UserLink.vue'
import MarkdownEditor from './MarkdownEditor.vue';
export default {
  components: { UserLink, MarkdownEditor },
  name: "SearchResultBook",
  props: {
    post: {
      type: Object,
      required: true,
    },
    topic: {
      type: String,
      required: false,
    }
  },
  computed: {
      ...mapGetters("global", ["isRepostingAllowed", "activeDiscussionId"])
  }
}
</script>