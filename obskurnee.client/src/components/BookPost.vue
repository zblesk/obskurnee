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
        <div v-if="(isMe(post.ownerId) || isMod) && !editMode">
          <span class="book__edit" @click="startEditing()">
            📝
          </span>
        </div>
        <div v-if="editMode">
          <div class="form-field">
            <label for="name" v-if="topic == 'Books' || topic == 'Recommendations'">{{$t('newpost.bookTitle')}}*</label>
            <label for="name" v-if="topic == 'Themes'">{{$t('newpost.topicTitle')}}*</label>
            <input v-model="editedPost.title" id="name" required />
          </div>
          <div class="form-field" v-if="topic != 'Themes'">
            <label for="author">{{$t('newpost.authorName')}}*</label>
            <input v-model="editedPost.author" id="autor" required  />
          </div>
          <div class="form-field" v-if="topic != 'Themes'">
            <label for="pages">{{$t('newpost.pageCount')}}*</label>
            <input type="number" v-model="editedPost.pageCount" id="pages" required />
          </div>
          <markdown-editor v-model="editedPost.text" :required="topic != 'Themes'"></markdown-editor>
          <div class="buttons u-mt-sp">
            <button @click="saveChanges()" class="button-primary">{{$t('menus.save')}}</button>
            <button @click="cancelEditing()" class="button-secondary edit-cancel">{{$t('menus.cancel')}}</button>
          </div>
        </div>
        <div v-else>
          <a :href="post.url" class="book__link">
            <h2 class="book__title">{{ post.title }}</h2>
          </a> 
          <p v-if="post.author" class="book__author">{{ post.author }}</p>
          <p class="book__pages" v-if="post.pageCount">{{$t('bookpost.numPages', [post.pageCount])}}</p>
          <p class="book__owner">{{$t('bookpost.suggestedBy')}} <user-link :userId="post.ownerId" :userName="post.ownerName" /></p>
          <div class="book__text" v-html="post.renderedText"> </div>
        </div>
      </div>
      <div class="book__repost" v-if="isRepostingAllowed 
        && activeDiscussionId != post.discussionId
        && topic == 'Books'">
        <router-link 
          :to="{ name: 'discussion', params: { discussionId: activeDiscussionId }, 
          query: { parentPostId: post.postId, fromDiscussionId: post.discussionId } }">
            <button class="button-secondary button-repost">{{$t('bookpost.addToOngoing')}}</button>
        </router-link>
      </div>
</div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import UserLink from './UserLink.vue'
import MarkdownEditor from './MarkdownEditor.vue';
export default {
  components: { UserLink, MarkdownEditor },
  name: "BookPost",
  data() {
    return {
      editMode: false,
      editedPost: {}
    };
  },
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
    ...mapGetters("global", ["isRepostingAllowed", "activeDiscussionId"]),
    ...mapGetters("context", ["isMe", "isMod"]),
  },
  methods: {
    ...mapActions("discussions", ["updatePost"]),
    startEditing()
    {
      this.editMode = true;
      this.editedPost = { ...this.post };
    },
    cancelEditing()
    {
      this.editMode = false;
      this.editedPost = {};
    },
    async saveChanges()
    {
      try
      {
        await this.updatePost({ post: this.editedPost });
        this.editMode = false;
        this.editedPost = {};
      }
      catch (ex)
      {
        console.log(ex);
        this.$notifyError(this.$t('messages.updateFailed'));
      }
    }
  }
}
</script>

<style scoped>
    .edit-cancel {
        margin-top: var(--spacer);
    }

    @media screen and (min-width: 576px) {
        .edit-cancel {
            margin-top: 0;
            margin-left: var(--spacer);
        }
    }
</style>