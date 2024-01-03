<template>
  <div class="book">
    <div class="book__top" v-if="recommendation">
      <div class="book__cover" v-if="recommendation.imageUrl">
        <a :href="recommendation.url" class="book__link">
          <img :src="recommendation.imageUrl" :alt="recommendation.title">
        </a>
      </div>
      <router-link class="book__permalink" v-if="recommendation.recommendationId"
        :to="{ name: 'singlerecommendation', params: { recommendationId: recommendation.recommendationId } }">🔗
      </router-link> 
      <div v-if="isMe(recommendation.ownerId) || isMod">
        <span v-if="editMode" class="book__edit" @click="saveChanges()">
          💾
        </span>
        <span v-else class="book__edit" @click="startEditing()">
          📝
        </span>
      </div>
      <div v-if="editMode">
        <div class="form-field">
          <label for="name">{{$t('newpost.bookTitle')}}*</label>
          <input v-model="editedRec.title" id="name" required />
        </div>
        <div class="form-field">
          <label for="author">{{$t('newpost.authorName')}}*</label>
          <input v-model="editedRec.author" id="autor" required  />
        </div>
        <div class="form-field">
          <label for="pages">{{$t('newpost.pageCount')}}*</label>
          <input type="number" v-model="editedRec.pageCount" id="pages" required />
        </div>
        <markdown-editor v-model="editedRec.text" :required="true"></markdown-editor>
      </div>
      <div v-else>
        <a :href="recommendation.url" class="book__link">
          <h2 class="book__title">{{ recommendation.title }}</h2>
        </a>
        <p v-if="recommendation.author" class="book__author">{{ recommendation.author }}</p>
        <p class="book__pages" v-if="recommendation.pageCount">{{$t('bookpost.numPages', [recommendation.pageCount])}}</p>
        <p class="book__owner" v-if="showName">{{$t('recommendations.recommendedBy')}}
          <user-link v-if="recommendation.ownerId" :userId="recommendation.ownerId" :userName="recommendation.ownerName" />
        </p>
        <p class="book__text" v-html="recommendation.renderedText"> </p>
      </div>
      <div class="book__repost" v-if="isRepostingAllowed">
        <router-link v-if="isRepostingAllowed" 
          :to="{ name: 'discussion', params: { discussionId: activeDiscussionId }, 
          query: { parentRecommendationId: recommendation.recommendationId } }">
          <button class="button-secondary button-repost">{{$t('bookpost.addToOngoing')}}</button>
        </router-link>
      </div>
    </div>
  </div> 
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import UserLink from './UserLink.vue';
import MarkdownEditor from "./MarkdownEditor.vue";
export default {
  components: { UserLink, MarkdownEditor },
  name: "RecommendationCard",
  props: {
    recommendation: {
      type: Object,
      required: true,
    },
    showName: {
      type: Boolean,
      required: false,
      default: true,
    }
  },
  data() {
    return {
      editMode: false,
      editedRec: {}
    };
  },
  computed: {
    ...mapGetters("global", ["isRepostingAllowed", "activeDiscussionId"]),
    ...mapGetters("context", ["isMe", "isMod"]),
  },
  methods: {
    ...mapActions("recommendations", ["updateRecommendation"]),
    startEditing() {
      this.editMode = true;
      this.editedRec = this.recommendation;
    },
    async saveChanges() {
      try {
        await this.updateRecommendation({ rec: this.editedRec });
        this.editMode = false;
        this.editedRec = {};
      } catch (ex) {
        console.log(ex);
        this.$notifyError(this.$t('messages.updateFailed'));
      }
    }
 }
}
</script>
